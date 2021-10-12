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
string with_rbk = false ? "true" : "false";

var finderWrapper = new FinderWrapper(pythonPath, scriptPath);

int res = await finderWrapper.StartAsync(sites_file, output_path, timeout);
Console.WriteLine(res);