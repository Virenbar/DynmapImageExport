using DynmapImageExport.Models;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Text.RegularExpressions;
using static DynmapImageExport.Commands.Common;
using Padding = DynmapImageExport.Models.Padding;

namespace DynmapImageExport.Commands
{
    internal class MergeCommand : Command
    {
        public MergeCommand() : base("merge", "Merge dynmap to image")
        {
            AddAlias("m");
            AddArgument(new Argument<string>("url", "Dynmap URL"));
            AddArgument(new Argument<string>("world", "World name"));
            AddArgument(new Argument<string>("map", "Map name"));
            AddArgument(new Argument<string>("center", () => "[0,64,0]", "Center of image [x,y,z]"));
            AddArgument(new Argument<string>("range", () => "[2]", "Range of image in tiles [all]|[vert,horz]|[top,right,bottom,left]"));
            AddArgument(new Argument<int>("zoom", () => 0, "Zoom"));
            AddOption(new Option<string>(new[] { "--output", "-o" }, "Output path"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        private static async Task<int> HandleCommand(string URL, string world, string map, string center, string range, int? zoom, string output)
        {
            try
            {
                AnsiConsole.MarkupLine($"[yellow]Merging of: {URL} - {world} - {map}[/]");
                var D = await GetDynmap(URL);

                if (!D.Worlds.ContainsKey(world)) { throw new ArgumentException($"Invalid world name: {world}", nameof(world)); }
                if (!D.Maps.ContainsKey((world, map))) { throw new ArgumentException($"Invalid map name: {map}", nameof(map)); }
                //
                var World = D.Worlds[world];
                var Map = D.Maps[(world, map)];
                var Source = new TileSource(D, World, Map);
                //
                var Center = Point.Parse(center);
                var Range = Padding.Parse(range);
                var Zoom = zoom ?? (int)Math.Log(Map.Scale, 2);

                var CenterTile = Source.TileAtPoint(Center, Zoom);
                var Tiles = CenterTile.CreateTileMap(Range);

                AnsiConsole.MarkupLine($"Center point: {Center}");
                AnsiConsole.MarkupLine($"Central tile: {CenterTile}");
                AnsiConsole.MarkupLine($"Range size: {Range.Height}x{Range.Width}");
                AnsiConsole.MarkupLine($"Tiles to download: {Tiles.Count}");
                // Download
                var Images = await Download(Tiles);
                AnsiConsole.MarkupLine($"Tiles downloaded: {Images.Count}");
                // Merge
                var path = output ?? $"{D.Config.Title} ({world}-{map}-{center}-{range}-{zoom})";
                var image = Merge(Images, path);
                AnsiConsole.MarkupLineInterpolated($"[yellow]Merged image saved to: {image.FullName}[/]");
                return 0;
            }
            catch (ArgumentException E)
            {
                AnsiConsole.WriteException(E, ExceptionFormats.ShortenPaths);
                return 1;
            }
        }

        #region Tasks

        private static async Task<ImageMap> Download(TileMap range)
        {
            using var TD = new TileDownloader(range);
            return await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading tiles: 0/{range.Count}", true, range.Count);
                    var PP = GetProgress(T);
                    return await TD.Download(PP);
                });
        }

        private static IProgress<string> GetProgress(ProgressTask task)
        {
            var PP = new Progress<string>(str =>
            {
                task.Increment(1);
                task.Description = Regex.Replace(task.Description, "\\d+/\\d+", $"{task.Value}/{task.MaxValue}");
            });
            return PP;
        }

        private static FileInfo Merge(ImageMap images, string path)
        {
            path = path.Replace(".png", "", StringComparison.InvariantCultureIgnoreCase) + ".png";
            var M = new TileMerger(images, path);

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var T = ctx.AddTask($"Merging images: 0/{images.Count}", true, images.Count);
                    var PP = GetProgress(T);
                    M.Merge(PP);
                });
            return new FileInfo(path);
        }

        #endregion Tasks
    }
}