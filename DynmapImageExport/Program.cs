using DynmapImageExport;
using DynmapImageExport.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

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
Parser.Invoke("i https://map.minecrafting.ru/ world flat -v");
Parser.Invoke("m https://map.minecrafting.ru/ world flat [-70,64,-70] [10,10] 2 -v");
Parser.Invoke("m https://map.minecrafting.ru/ world se_view [8057,138,4492] [2,2]");

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
        AnsiConsole.WriteLine();
    }
}