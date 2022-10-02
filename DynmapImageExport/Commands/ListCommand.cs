using Dynmap;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace DynmapTools.Commands
{
    internal class ListCommand : Command
    {
        public ListCommand() : base("list", "Show available worlds and maps")
        {
            AddAlias("ls");
            AddArgument(new Argument<string>("url", "Dynmap URL"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(string URL)
        {
            var D = new DynMap(URL);
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("[yellow]Getting map list...[/]", async ctx => { await D.RefreshConfig(); });

            var Worlds = D.Config.Worlds;
            var WNMax = Worlds.Max(W => W.Name.Length);
            var MNMax = Worlds.SelectMany(W => W.Maps).Max(M => M.Name.Length);

            var WTMax = Worlds.Max(W => W.Title.Length);
            var MTMax = Worlds.SelectMany(W => W.Maps).Max(M => M.Title.Length);

            var Root = new Tree("Available worlds");
            foreach (var W in Worlds)
            {
                var WNode = Root.AddNode($"[white]{W.Name} - {W.Title}[/]");
                foreach (var M in W.Maps)
                {
                    WNode.AddNode($"[yellow]{M.Name.PadRight(MNMax)} {M.Title}[/]");
                }
            }
            AnsiConsole.Write(Root);

            return 0;
        }
    }
}