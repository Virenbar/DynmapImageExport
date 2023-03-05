using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace DynmapImageExport.Commands
{
    internal class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Show info about map")
        {
            AddAlias("i");
            AddArgument(new Argument<Uri>("url", "Dynmap URL"));
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

        private async Task<int> HandleCommand(Uri URL, string world, string map)
        {
            AnsiConsole.MarkupLine($"[yellow]Info for: {URL.Host} - {world} - {map}[/]");
            var Dynmap = await Common.GetDynmap(URL);
            var World = Dynmap.GetWorld(world);
            var Map = World.GetMap(map);
            var MaxOut = Map.MapZoomOut;

            AnsiConsole.WriteLine($"World: {World.Name} - {World.Title}");
            AnsiConsole.WriteLine($"Map: {Map.Name} - {Map.Title}");
            AnsiConsole.MarkupLine($"[white]Perspective: {Map.Perspective} PPB: {Map.Scale}[/]");
            var Zoom = new Tree($"[white]Zoom levels[/]");
            for (int zoom = 0; zoom <= MaxOut; zoom++)
            {
                Zoom.AddNode($"[yellow]{zoom}[/][white] - {Map.ZoomToString(zoom)}[/]");
            }
            AnsiConsole.Write(Zoom);

            return 0;
        }
    }
}