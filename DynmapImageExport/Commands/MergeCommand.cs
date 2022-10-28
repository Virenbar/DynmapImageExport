using DynmapImageExport.Models;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;
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
            AddArgument(new Argument<Uri>("url", "Dynmap URL"));
            AddArgument(new Argument<string>("world", "World name"));
            AddArgument(new Argument<string>("map", "Map name"));
            AddArgument(new Arguments.Center());
            AddArgument(new Arguments.Range());
            AddArgument(new Argument<int>("zoom", () => 0, "Zoom"));
            AddOption(new Option<string>(new[] { "--output", "-o" }, "Output path"));
            AddOption(new Option<bool>(new[] { "--no-cache", "-nc" }, "Ignore cached tiles"));
            Handler = CommandHandler.Create(HandleCommand);
        }

        private static async Task<int> HandleCommand(
            Uri URL,
            string world,
            string map,
            Point center,
            Padding range,
            int? zoom,
            string output,
            bool noCache)
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
                var Zoom = zoom ?? (int)Math.Log(Map.Scale, 2);
                var CenterTile = Source.TileAtPoint(center, Zoom);
                var Tiles = CenterTile.CreateTileMap(range);

                var Info = $"""
                    Center point: {center}
                    Central tile: {CenterTile}
                    Range size: {range.Height}x{range.Width}
                    """;
                Trace.WriteLine(Info);
                AnsiConsole.WriteLine(Info);
                // Download
                AnsiConsole.WriteLine($"Tiles to download: {Tiles.Count}");
                var Images = await Download(Tiles, !noCache);
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

        private static async Task<ImageMap> Download(TileMap range, bool useCache)
        {
            using var TD = new TileDownloader(range) { UseCache = useCache };
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
            path = Regex.Replace(path, @"\.\w{3,4}$", "", RegexOptions.IgnoreCase) + ".png";
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