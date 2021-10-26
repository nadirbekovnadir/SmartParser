using Models;

var ps = new ParserService();

var tokens = ps.Decompose("((0 @ 1 f) # (2 @ 3) @ 4)");
var result = ps.Compute(new List<bool> { false, false, true, false, false});

Console.WriteLine();