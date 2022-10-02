using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using static DynmapImageExport.Commands.Common;

namespace DynmapImageExport.Commands
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
            var D = await GetDynmap(URL);
            var Worlds = D.Config.Worlds;
            var MNMax = Worlds.SelectMany(W => W.Maps).Max(M => M.Name.Length);

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