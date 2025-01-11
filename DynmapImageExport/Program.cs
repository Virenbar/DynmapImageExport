using DynmapImageExport.Commands;
using DynmapImageExport.Extensions;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace DynmapImageExport
{
    public static class Program
    {
        private static readonly Parser Parser;

        static Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var RootCommand = new RootCommand("Dynmap Image Export") {
                new AboutCommand(),
                new ListCommand(),
                new InfoCommand(),
                new MergeCommand()
            };

            Parser = new CommandLineBuilder(RootCommand)
               .UseTrace()
               .UseParseErrorReporting()
               .UseExceptionHandler(ExceptionHandler.Handle)
               .UseHelp()
               .Build();
        }

        internal static int Invoke(string[] args)
        {
            args = args.FixBrackets();
            return Parser.Invoke(args);
        }

        internal static int Invoke(string args)
        {
            args = args.RemoveName();
            args = args.FixBrackets();
            return Parser.Invoke(args);
        }

        private static int Main(string[] args)
        {
#if DEBUG
            Debug.Invoke();
#endif
            if (args.Length > 0)
            {
                return Invoke(args);
            }
            else
            {
                Parser.Invoke("-h");
                while (true)
                {
                    AnsiConsole.MarkupLine("[green]Input command:[/]");
                    var Input = AnsiConsole.Prompt(new TextPrompt<string>("[green]>[/]"));
                    AnsiConsole.WriteLine();
                    Invoke(Input);
                }
            }
        }
    }
}