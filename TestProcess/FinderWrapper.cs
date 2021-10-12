using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestProcess
{
    public class FinderWrapper
    {
        public string PythonPath { get; private set; }
        public string ScriptPath {  get; private set; }

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

        public FinderWrapper(string pythonPath, string scriptPath)
        {
            PythonPath = pythonPath;
            ScriptPath = scriptPath;

            ProcessInfo = new ProcessStartInfo();

            ProcessInfo.FileName = PythonPath;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.CreateNoWindow = true;

            ProcessInfo.RedirectStandardInput = true;
            ProcessInfo.RedirectStandardOutput = true;
            ProcessInfo.RedirectStandardError = true;

            State = ProcessState.Started;
        }

        public Task<int> StartAsync(
            string inputPath,string outputPath, int timeout = 15, bool with_rbk = false)
        {
            var tcs = new TaskCompletionSource<int>();

            var timeoutArg = timeout.ToString();
            var withRbkArg = with_rbk ? "true" : "false";

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
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.OutputDataReceived += Process_OutputDataReceived;

            process.Start();
            process.BeginOutputReadLine();

            return tcs.Task;
        }

        private int n_loadingLinks = 0;
        private List<string> loadedLinks = new();
        private List<string> brokenLinks = new();
        private int n_loadedLinks = 0;

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var output = e.Data ?? "";

            switch (State)
            {
                case ProcessState.Started:
                    State = StateStart(output);
                    break;
                case ProcessState.Loading:
                    State = StateLoading(output);
                    break;
                case ProcessState.LoadingCompleted:
                    State = StateLoadingCompleted(output);
                    break;
                case ProcessState.Parsing:
                    State = StateParsing(output);
                    break;
                case ProcessState.ParsingCompleted:
                    State = StateParsingCompleted(output);
                    break;
                case ProcessState.Completed:
                    break;
                case ProcessState.None:
                    break;
                default:
                    break;
            }

            return;
        }


        Regex noneRegex = new(
            @"^!Loading\[(?<n>\d+)\]",
            RegexOptions.Compiled);
        private ProcessState StateStart(string output)
        {
            var match = noneRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            Console.WriteLine(output);

            loadedLinks.Clear();
            n_loadingLinks = Convert.ToInt32(match.Groups["n"].Value);

            return ProcessState.Loading;
        }

        Regex loadingRegex = new(
            @"(?<Completed>Completed\[(?<n>\d+)\])|(?<Loaded>Loaded\[(?<link>.+)\])|(?<Broken>Broken\[(?<link>.+)\])",
            RegexOptions.Compiled);
        private ProcessState StateLoading(string output)
        {
            var match = loadingRegex.Match(output);
            if (!match.Success)
                return ProcessState.None;

            Console.WriteLine(output);

            if (match.Groups["Completed"].Success)
            {
                n_loadingLinks = Convert.ToInt32(match.Groups["n"].Value);

                return ProcessState.LoadingCompleted;
            }

            var link = match.Groups["link"].Value;

            if (match.Groups["Loaded"].Success)
                loadedLinks.Add(link);
            else
                brokenLinks.Add(link);

            return ProcessState.Loading;
        }

        private ProcessState StateLoadingCompleted(string output)
        {
            Console.WriteLine(output);
            return ProcessState.Parsing;
        }

        private ProcessState StateParsing(string output)
        {
            Console.WriteLine(output);
            //TODO логика переключения 

            return ProcessState.Parsing;
        }

        private ProcessState StateParsingCompleted(string output)
        {
            Console.WriteLine(output);
            return ProcessState.Completed;
        }
    }
}
