using Dynmap;
using DynmapTools.Models;
using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Text.RegularExpressions;
using Padding = DynmapTools.Models.Padding;

namespace DynmapTools.Commands
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
            AddArgument(new Argument<string>("range", "Range of image in tiles [all]|[vert,horz]|[top,left,bottom,right]"));
            AddArgument(new Argument<int>("zoom", () => 0, "Zoom"));
            AddOption(new Option<string>(new[] { "--output", "-o" }, "Output path"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        private static async Task<int> HandleCommand(string URL, string world, string map, string center, string range, int? zoom, string output)
        {
            try
            {
                var D = new DynMap(URL);
                await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .Start("[yellow]Getting map list...[/]", async ctx => { await D.RefreshConfig(); });
                if (!D.Worlds.ContainsKey(world)) { throw new ArgumentException($"Invalid world name: {world}", nameof(world)); }
                if (!D.Maps.ContainsKey((world, map))) { throw new ArgumentException($"Invalid map name: {map}", nameof(map)); }
                //
                var World = D.Worlds[world];
                var Map = D.Maps[(world, map)];
                var Source = new TileSource(D, World, Map);
                //
                var Center = Point.Parse(center);
                var Range = Padding.Parse(range);
                var Zoom = zoom ?? Math.Log(Map.Scale, 2);
                AnsiConsole.MarkupLine($"Center point: {Center}");
                AnsiConsole.MarkupLine($"Range size: {Range.Width}x{Range.Height}");

                var CenterTile = Source.TileAtPoint(Center, 0);
                var Tiles = CenterTile.CreateTileRange(Range);
                AnsiConsole.MarkupLine($"Central tile: {CenterTile}");
                AnsiConsole.MarkupLine($"Tiles to download: {Tiles.Count}");
                // Download
                var Images = await Download(Tiles);
                AnsiConsole.MarkupLine($"Tiles downloaded: {Images.Count}");
                // Merge
                var path = output ?? $"{D.Config.Title}-{DateTime.Now:yyyy-MM-dd HH.mm.ss}";
                Merge(Images, path);
                //AnsiConsole.MarkupInterpolated($"[green]{URL} {world} {range} {map} {zoom}[/]");
                return 0;
            }
            catch (ArgumentException E)
            {
                //AnsiConsole.MarkupLine($"[red]{E.Message.EscapeMarkup()}[/]");
                AnsiConsole.WriteException(E, ExceptionFormats.ShortenPaths);
                return 1;
            }
        }

        private static async Task<ImageMap> Download(TileMap range)
        {
            using var TD = new TileDownloader(range);
            return await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading tiles: 0/{range.Count}", true, range.Count);
                    var PP = new Progress<string>(str =>
                    {
                        T.Increment(1);
                        T.Description = Regex.Replace(T.Description, "\\d+/\\d+", $"{T.Value}/{T.MaxValue}");
                    });
                    return await TD.Download(PP);
                });
        }

        private static void Merge(ImageMap images, string path)
        {
            path = path.Replace(".png", "", StringComparison.InvariantCultureIgnoreCase) + ".png";
            var M = new Merger(images, path);

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var T = ctx.AddTask($"Merging images: 0/{images.Count}", true, images.Count);
                    var PP = new Progress<string>(str =>
                    {
                        T.Increment(1);
                        T.Description = Regex.Replace(T.Description, "\\d+/\\d+", $"{T.Value}/{T.MaxValue}");
                    });
                    M.Merge(PP);
                });
        }
    }
}