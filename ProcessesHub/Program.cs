// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using TestProcess;

var appLocation = AppDomain.CurrentDomain.BaseDirectory ?? "";
string pythonPath = Path.Combine(appLocation, "parser_env/Scripts/python.exe");

// Args
string scriptPath = Path.Combine(appLocation, "FinderProcess.py");
string sites_file = "C:/Users/Nadir/Documents/Parser_data/sites_test.txt";
string output_path = "C:/Users/Nadir/Documents/Parser_data";
int timeout = 15;
bool with_rbk = false;

var finderWrapper = new FinderProcess(pythonPath, scriptPath);

finderWrapper.LoadingStarted += (s, e) => { Console.WriteLine($"Loading started [{e.SourcesCount}]"); };
finderWrapper.SourceLoaded += (s, e) => { Console.Write("#"); };
finderWrapper.LoadingCompleted += (s, e) => { Console.WriteLine($"\nLoading completed [{e.SourcesCount}]\n"); };

finderWrapper.ParsingStarted += (s, e) => { Console.WriteLine($"Parsing started [{e.SourcesCount}]"); };
finderWrapper.SourceParsed += (s, e) => { 
    Console.Write($"{e.Value?.Name}[{e.Value?.EntitesCount}]");

    string cols = "";

    if (e.Value is not null)
    {
        foreach (var col in e.Value.MissedColumns)
        {
            cols += col + " ";
        }
    }

    Console.WriteLine($": {cols}");
};
finderWrapper.ParsingCompleted += (s, e) => { Console.WriteLine($"Loading completed [{e.SourcesCount}][{e.EntitiesCount}]\n"); };

int res = await finderWrapper.StartAsync(sites_file, output_path, timeout, with_rbk);
Console.WriteLine(res);