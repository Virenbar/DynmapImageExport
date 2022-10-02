using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using static DynmapImageExport.Commands.Common;

namespace DynmapImageExport.Commands
{
    internal class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Show info about map")
        {
            AddAlias("i");
            AddArgument(new Argument<string>("url", "Dynmap URL"));
            AddArgument(new Argument<string>("world", "World"));
            AddArgument(new Argument<string>("map", "Map"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        /*
         * scale - Number of pixels per block on max zoom (4 for lowres)
         * = 1:1 * (scale/(2^zoom))
         * 0 - 4:1 - '' = 4
         * 1 - 2:1 - 'z' = 2
         * 2 - 1:1 - 'zz' = 1
         * 3 - 1:2 - 'zzz' = 0.5
         * Min 'z' = ''
         * Max 'z' = 'z' * MapZoomOut //(3+ExtraZoomOut)? - 3 is default
         */

        private async Task<int> HandleCommand(string URL, string world, string map)
        {
            var D = await GetDynmap(URL);

            if (!D.Worlds.ContainsKey(world)) { throw new ArgumentException($"Invalid world name: {world}", nameof(world)); }
            if (!D.Maps.ContainsKey((world, map))) { throw new ArgumentException($"Invalid map name: {map}", nameof(map)); }

            var World = D.Worlds[world];
            var Map = D.Maps[(world, map)];
            var MaxOut = Map.MapZoomOut;
            var Scale = Map.Scale;

            AnsiConsole.WriteLine($"World: {World.Name} - {World.Title}");
            AnsiConsole.WriteLine($"Map: {Map.Name} - {Map.Title}");
            AnsiConsole.MarkupLine($"[white]Perspective: {Map.Perspective}[/]");
            AnsiConsole.MarkupLine($"[white]PPB: {Scale}[/]");
            var Zoom = new Tree($"[white]Zoom levels[/]");
            for (int zoom = 0; zoom <= MaxOut; zoom++)
            {
                var S = Scale / Math.Pow(2, zoom);
                Zoom.AddNode($"[yellow]{zoom} - {S.ToScale()}[/]");
            }
            AnsiConsole.Write(Zoom);

            return 0;
        }
    }
}