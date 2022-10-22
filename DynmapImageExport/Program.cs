using DynmapImageExport;
using DynmapImageExport.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var RootCommand = new RootCommand("Dynmap Image Export") {
    new ListCommand(),
    new InfoCommand(),
    new MergeCommand()
};

var Parser = new CommandLineBuilder(RootCommand)
    .UseVerbose()
    .UseParseErrorReporting()
    .UseExceptionHandler()
    .UseHelp()
    .Build();

#if DEBUG
Parser.Invoke("ls https://map.minecrafting.ru");
AnsiConsole.WriteLine("<==========>");
Parser.Invoke("i https://map.minecrafting.ru/ world flat");
AnsiConsole.WriteLine("<==========>");
Parser.Invoke("m https://map.minecrafting.ru/ world flat [0,100,0] [5,6,5,5] 2");
AnsiConsole.WriteLine("<==========>");
Parser.Invoke("m https://map.minecrafting.ru/ world se_view [0,100,0] [5,11,5,10]");

//Parser.Invoke("m https://map.minecrafting.ru/ world flat [2,2,2] [2,2]");
//https://ebs.virenbar.ru/dynmap/
#endif

if (args.Length > 0) { Parser.Invoke(args); }
else
{
    Parser.Invoke("-h");
    while (true)
    {
        AnsiConsole.MarkupLine("[green]Input command:[/]");
        var Input = AnsiConsole.Prompt(new TextPrompt<string>("[green]>[/]"));
        Input = Input.Replace("DynmapImageExport", "");
        AnsiConsole.WriteLine();
        Parser.Invoke(Input);
    }
}