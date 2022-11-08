using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;

namespace DynmapImageExport.Commands
{
    internal class AboutCommand : Command
    {
        public AboutCommand() : base("about", "Show info about application")
        {
            AddAlias("a");

            Handler = CommandHandler.Create(HandleCommand);
        }

        private int HandleCommand()
        {
            var G = new Grid() { Width = 60 };
            G.AddColumns(2);
            var P = new Panel(G)
            {
                Header = new("[yellow]DYNMAP IMAGE EXPORT[/]", Justify.Center)
            };
            AnsiConsole.Live(P).Start(ctx =>
            {
                Thread.Sleep(500);
                G.AddRow("Made by:", "[green]Virenbar[/]");
                ctx.Refresh();
                Thread.Sleep(500);
                G.AddRow("Source:", "[white][link]https://github.com/Virenbar/DynmapImageExport[/][/]");
                ctx.Refresh();
                Thread.Sleep(500);
                G.AddRow("Version:", $"[yellow]{Assembly.GetExecutingAssembly().GetName().Version}[/]");
                ctx.Refresh();
            });
            return 0;
        }
    }
}