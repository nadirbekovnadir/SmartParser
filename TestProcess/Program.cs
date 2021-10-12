// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Reflection;

//string? execPath = Assembly.GetEntryAssembly().Location;
//Console.WriteLine(execPath);

var appLocation = AppDomain.CurrentDomain.BaseDirectory ?? "";
string pythonPath = Path.Combine(appLocation, "parser_env/Scripts/python.exe");

// Args
string scriptPath = Path.Combine(appLocation, "FinderProcess.py");
string sites_file = "C:/Users/Nadir/Documents/Parser_data/sites_test.txt";
string output_path = "C:/Users/Nadir/Documents/Parser_data";
string timeout = 15.ToString();
string with_rbk = false ? "true" : "false";

var processStartInfo = new ProcessStartInfo();

processStartInfo.FileName = pythonPath;
processStartInfo.Arguments = 
    $"\"{scriptPath}\" \"{sites_file}\" \"{output_path}\" \"{timeout}\" \"{with_rbk}\"";

processStartInfo.UseShellExecute = false;
processStartInfo.CreateNoWindow = true;

processStartInfo.RedirectStandardInput = true;
processStartInfo.RedirectStandardOutput = true;
processStartInfo.RedirectStandardError = true;

var process = new Process();
process.StartInfo = processStartInfo;

process.OutputDataReceived += (s, e) => { Console.WriteLine(e.Data); };

process.Start();
process.BeginOutputReadLine();

process.WaitForExit();