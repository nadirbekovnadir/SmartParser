// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Models.Entities;
using Models;

ParserService parserService = new ParserService();

//parserService.Decomposite(";huo uygo pihup");
var result = parserService.Decomposite("hihovuio @ ivovobpb onpnp # oyfvit");
var result2 = parserService.Compute(new List<bool>{true, false, true});
Console.WriteLine();