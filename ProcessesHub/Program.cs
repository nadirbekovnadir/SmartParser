// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Models.Entities;
using Models;

var appLocation = AppDomain.CurrentDomain.BaseDirectory ?? "";
string pythonPath = Path.Combine(appLocation, "parser_env/Scripts/python.exe");

// Args
string scriptPath = Path.Combine(appLocation, "FinderProcess.py");
string sites_file = "C:/Users/Nadir/Documents/Parser_data/sites_test.txt";
string output_path = "C:/Users/Nadir/Documents/Parser_data/Extracted";

if (!Directory.Exists(output_path))
    Directory.CreateDirectory(output_path);

int timeout = 30;
bool with_rbk = false;

var extractor = new NewsExtractor();

extractor.LoadingStarted += (s, e) => { Console.WriteLine($"Loading started [{e.SourcesCount}]"); };
extractor.SourceLoaded += (s, e) => { Console.Write(e.Success ? "#" : "_"); };
extractor.LoadingCompleted += (s, e) => { Console.WriteLine($"\nLoading completed [{e.SourcesCount}]\n"); };

extractor.ParsingStarted += (s, e) => { Console.WriteLine($"Parsing started [{e.SourcesCount}]"); };
extractor.SourceParsed += (s, e) => { 
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
extractor.ParsingCompleted += (s, e) => { Console.WriteLine($"Loading completed [{e.SourcesCount}][{e.EntitiesCount}]\n"); };

//int res = await extractor.StartAsync(sites_file, output_path, timeout, with_rbk);
//Console.WriteLine(res);

//Console.WriteLine("Loading Entities");

//var entities = NewsEntity.LoadFromCsv("C:\\Users\\Nadir\\Documents\\Parser_data\\Extracted\\2021-10-15_18-43-54.csv");
//var olderEntities = NewsEntity.LoadFromCsv("C:\\Users\\Nadir\\Documents\\Parser_data\\Extracted\\2021-10-15_18-38-19.csv");

//var match = DataHelper.Match(entities, @"ДолЛАР|РубЛ|ЕвРО");
//NewsEntity.SaveToCsv(match, "C:\\Users\\Nadir\\Documents\\Parser_data\\Extracted\\Match.csv");

//var except = DataHelper.Except(entities, olderEntities);
//NewsEntity.SaveToCsv(except, "C:\\Users\\Nadir\\Documents\\Parser_data\\Extracted\\Except.csv");

//Console.WriteLine("Completed");