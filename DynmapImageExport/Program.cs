using DynmapImageExport.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Text;
using System.Text.RegularExpressions;

namespace DynmapImageExport
{
    public static class Program
    {
        private static readonly Regex Brackets = new(@"\s+(?=[^[\]]*\])");
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

        internal static int Invoke(string[] args) => Invoke(string.Join(' ', args));

        internal static int Invoke(string args)
        {
            args = args.Replace("DynmapImageExport", "");
            args = Brackets.Replace(args, "");
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
                Invoke("-h");
                while (true)
                {
                    AnsiConsole.MarkupLine("[green]Input command:[/]");
                    var Input = AnsiConsole.Prompt(new TextPrompt<string>("[green]>[/]"));
                    Input = Input.Replace("DynmapImageExport", "");
                    AnsiConsole.WriteLine();
                    Parser.Invoke(Input);
                }
            }
        }
    }
}