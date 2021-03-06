using SmartParser.Domain.Entities;
using SmartParser.Domain.Services.Common;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SmartParser.Domain.Services
{

	public class NewsExtractor : INewsExtractor
    {
        public string ApplicationName { get; private set; }
        public string PythonPath { get; private set; }
        public string ScriptPath { get; private set; }
        public List<NewsEntity> News { get; set; } = new List<NewsEntity>();

        public ProcessStartInfo ProcessInfo { get; private set; }

        public enum ProcessState
        {
            Started,
            Loading,
            LoadingCompleted,
            Parsing,
            ParsingCompleted,
            Completed,
            None
        }

        public ProcessState State { get; private set; }

        public NewsExtractor()
        {
            ApplicationName = Path.GetDirectoryName(AppContext.BaseDirectory) ?? "";
            PythonPath = Path.Combine(ApplicationName, "parser_venv\\Scripts\\python.exe");
            ScriptPath = Path.Combine(ApplicationName, "Scripts\\ExtractProcess.py");

            ProcessInfo = new ProcessStartInfo
            {
                FileName = PythonPath,
                UseShellExecute = false,
                CreateNoWindow = true,

                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        #region EventArgs

        public class LoadingEventArgs : EventArgs
        {
            public int SourcesCount { get; set; }
        }

        public class LoadedEventArgs : EventArgs
        {
            public bool Success { get; set; }
            public NewsSources? Value { get; set; }
        }

        public class ParsingEventArgs : EventArgs
        {
            public int SourcesCount { get; set; }
            public int EntitiesCount { get; set; }
        }

        public class ParsedEventArgs : EventArgs
        {
            public bool Success { get; set; }
            public NewsSources? Value { get; set; }
        }

        #endregion


        #region Events

        public event EventHandler? ProcessStarted;

        public event EventHandler<LoadingEventArgs>? LoadingStarted;
        public event EventHandler<LoadedEventArgs>? SourceLoaded;
        public event EventHandler<LoadingEventArgs>? LoadingCompleted;

        public event EventHandler<ParsingEventArgs>? ParsingStarted;
        public event EventHandler<ParsedEventArgs>? SourceParsed;
        public event EventHandler<ParsingEventArgs>? ParsingCompleted;

        public event EventHandler<CompletedEventArgs>? ProcessCompleted;

        #endregion


        #region Start

        public Task<int> StartAsync(
            string inputPath, int timeout = 15, bool with_rbk = false)
        {
            var tcs = new TaskCompletionSource<int>();

            var timeoutArg = timeout.ToString();
            var withRbkArg = with_rbk ? "true" : "false";

            string outputPath = Path.Combine(
                Directory.GetCurrentDirectory(), "temp");


            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
			else
			{
                var di = new DirectoryInfo(outputPath);
                foreach (var file in di.EnumerateFiles())
                    file.Delete();
            }

            ProcessInfo.Arguments =
                $"\"{ScriptPath}\" " +
                $"\"{inputPath}\" " +
                $"\"{outputPath}\" " +
                $"\"{timeoutArg}\" " +
                $"\"{withRbkArg}\"";

            var process = new Process
            {
                StartInfo = ProcessInfo,
                EnableRaisingEvents = true
            };


            process.Exited += (s, e) =>
            {
                State = ProcessState.Completed;
                FileInfo? file = null;

                try
                {
                    file = (from f in new DirectoryInfo(outputPath).EnumerateFiles()
                                orderby f.CreationTime descending
                                select f)
                                .FirstOrDefault();

                    if (file != null)
                        News = NewsEntity.LoadFromCsv(file.FullName);
                }
                catch (Exception ex)
                {
                    
                }

                file?.Delete();

                tcs.SetResult(process.ExitCode);
                ProcessCompleted?.Invoke(this, new CompletedEventArgs { ExitCode = process.ExitCode });
                process.Dispose();
            };

            process.OutputDataReceived += Process_OutputDataReceived;

            process.Start();

            State = ProcessState.Started;
            ProcessStarted?.Invoke(this, EventArgs.Empty);

            process.BeginOutputReadLine();

            return tcs.Task;
        }

        public int Start(
            string inputPath, int timeout = 15, bool with_rbk = false)
        {
            throw new NotImplementedException();
            //return 0;
        }

        #endregion


        //
        private readonly Dictionary<string, NewsSources> newsSources = new();

        //Loading
        private int n_loadingLinks = 0;
        private int n_loadedLinks = 0;

        //Parsing
        private int n_parsingLinks = 0;
        private int n_parsedLinks = 0;
        private int n_entities = 0;

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var output = e.Data ?? "";

            switch (State)
            {
                case ProcessState.Started:
                    State = FromStartState(output);
                    break;
                case ProcessState.Loading:
                    State = FromLoadingState(output);
                    break;
                case ProcessState.LoadingCompleted:
                    State = FromLoadingCompletedState(output);
                    break;
                case ProcessState.Parsing:
                    State = FromParsingState(output);
                    break;
                case ProcessState.None:
                    break;
                default:
                    break;
            }

            return;
        }


        #region StateSwitching

        private readonly Regex startRegex = new(
            @"^!Loading\[(?<n>\d+)\]",
            RegexOptions.Compiled);
        private ProcessState FromStartState(string output)
        {
            var match = startRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            newsSources.Clear();
            n_loadingLinks = Convert.ToInt32(match.Groups["n"].Value);

            LoadingStarted?.Invoke(this, new LoadingEventArgs { SourcesCount = n_loadingLinks });
            return ProcessState.Loading;
        }

        private readonly Regex loadingRegex = new(
            @"(?<Completed>^!Completed\[(?<n>\d+)\])|(?<Loaded>Loaded\[(?<name>.+)\]\[(?<url>.+)\])|(?<Broken>Broken\[(?<name>.+)\]\[(?<url>.+)\])",
            RegexOptions.Compiled);
        private ProcessState FromLoadingState(string output)
        {
            var match = loadingRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            if (match.Groups["Completed"].Success)
            {
                n_loadedLinks = Convert.ToInt32(match.Groups["n"].Value);

                LoadingCompleted?.Invoke(this, new LoadingEventArgs { SourcesCount = n_loadedLinks });
                return ProcessState.LoadingCompleted;
            }

            var name = match.Groups["name"].Value;
            var link = match.Groups["url"].Value;

            var ns = new NewsSources
            {
                Name = name,
                Url = link
            };

            if (match.Groups["Loaded"].Success)
                ns.IsLoaded = true;

            newsSources.Add(ns.Name, new NewsSources(ns));

            SourceLoaded?.Invoke(this, new LoadedEventArgs { Success = ns.IsLoaded, Value = new NewsSources(ns) });
            return ProcessState.Loading;
        }

        //!Parsing[{links_length - len(errors_links)}]
        private readonly Regex loadingCompletedRegex = new(
            @"^!Parsing\[(?<n>\d+)\]",
            RegexOptions.Compiled);
        private ProcessState FromLoadingCompletedState(string output)
        {
            var match = loadingCompletedRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            n_parsingLinks = Convert.ToInt32(match.Groups["n"].Value);

            ParsingStarted?.Invoke(this, new ParsingEventArgs { SourcesCount = n_parsingLinks });
            return ProcessState.Parsing;
        }

        //Parsed[{names[i]}][{entries_count}]:[{missed_cols}]
        private readonly Regex parsingRegex = new(
            @"(?<Completed>^!Completed\[(?<n>\d+)\]\[(?<n_entities>\d+)\])|(?<Parsed>Parsed\[(?<name>.+)\]\[(?<n>.+)\]:\[(?<missed>.*)\])|(?<Broken>Broken\[(?<name>.+)\]\[(?<n>.+)\])",
            RegexOptions.Compiled);
        private ProcessState FromParsingState(string output)
        {
            var match = parsingRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            if (match.Groups["Completed"].Success)
            {
                n_parsedLinks = Convert.ToInt32(match.Groups["n"].Value);
                n_entities = Convert.ToInt32(match.Groups["n_entities"].Value);

                ParsingCompleted?.Invoke(this, new ParsingEventArgs { EntitiesCount = n_entities, SourcesCount = n_parsedLinks });
                return ProcessState.ParsingCompleted;
            }

            var ns = newsSources[match.Groups["name"].Value];
            ns.EntitesCount = Convert.ToInt32(match.Groups["n"].Value);

            if (match.Groups["Parsed"].Success)
            {
                ns.IsParsed = true;

                var missedString = match.Groups["missed"].Value;
                var splitted = missedString.Split(", ");

                ns.MissedColumns = new List<string>();

                foreach (var s in splitted)
                    ns.MissedColumns.Add(s.Trim('\''));
            }

            SourceParsed?.Invoke(this, new ParsedEventArgs { Success = ns.IsParsed, Value = new NewsSources(ns) });
            return ProcessState.Parsing;
        }

        #endregion
    }
}
