using DynmapImageExport.Arguments;
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
            AddArgument(new PointArgument("center", "Center of image [x,y,z]"));
            AddArgument(new PaddingArgument("range", "Range of image in tiles [all]|[vert,horz]|[top,right,bottom,left]"));
            AddArgument(new Argument<int?>("zoom", () => null, "Zoom"));
            AddOption(new Option<string>(new[] { "--output", "-o" }, "Output path"));
            AddOption(new Option<bool>(new[] { "--no-cache", "-nc" }, "Ignore cached tiles"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        private static async Task<int> HandleCommand(Uri URL, string world, string map, Point center, Padding range, int? zoom,
            string output, bool noCache)
        {
            AnsiConsole.MarkupLine($"[yellow]Merging of: {URL.Host} - {world} - {map}[/]");
            var Dynmap = await GetDynmap(URL);
            var World = Dynmap.GetWorld(world);
            var Map = World.GetMap(map);
            var Source = new TileSource(Dynmap, World, Map);
            var Zoom = zoom ?? Map.ScaleToZoom(1);
            Map.ValidateZoom(zoom);

            var CenterTile = Source.TileAtPoint(center, Zoom);
            var Tiles = CenterTile.CreateTileMap(range);

            var Info = new Grid()
                .AddColumns(2)
                .AddRow("Center point:", $"{center}".EscapeMarkup())
                .AddRow("Central tile:", $"{CenterTile}".EscapeMarkup())
                .AddRow("Range size:", $"{range.Height} X {range.Width}")
                .AddRow("Tiles count:", $"~{Tiles.Count}")
                .AddRow("Image size:", $"~{range.Height * 128}px X ~{range.Width * 128}px");
            AnsiConsole.Write(Info);
            Trace.WriteLine($"Input: {world}-{map}-{center}-{range}-{zoom}");

            var path = output ?? $"{Tiles.Source.Title} ({world}-{map}-{center}-{range}-{zoom})";
            path = Regex.Replace(path, @"\.\w{3,4}$", "", RegexOptions.IgnoreCase) + ".png";

            var SW = new Stopwatch();
            SW.Start();
            using var Merger = await AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn()
                })
                .StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading tiles: 0/{Tiles.Count}", true, Tiles.Count);
                    var T2 = ctx.AddTask($"Merging tiles: 0/0", false);
                    T2.IsIndeterminate = true;

                    using var TD = new TileDownloader(Tiles, 8) { UseCache = !noCache };
                    var Images = await TD.Download(GetProgress(T));

                    T2.MaxValue = Images.Count;
                    T2.IsIndeterminate = false;
                    T2.StartTask();
                    var TM = new TileMerger(Images);
                    TM.Merge(GetProgress(T2));
                    return TM;
                });
            var Image = AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Saving image", ctx => Merger.Save(path));
            SW.Stop();
            var TP = new TextPath(Image.FullName)
                .LeafColor(Color.Yellow);
            var Out = new Grid()
                .AddColumns(2)
                .AddRow("[white]Merge done:[/]", $@"[yellow]{SW.Elapsed:hh\:mm\:ss}[/]")
                .AddRow(new Markup("[white]Image path:[/]"), TP);
            AnsiConsole.Write(Out);
            return 0;
        }

        private static IProgress<int> GetProgress(ProgressTask task)
        {
            var PP = new Progress<int>(i =>
            {
                task.Increment(1);
                task.Description = Regex.Replace(task.Description, @"\d+/\d+", $"{task.Value}/{task.MaxValue}");
            });
            return PP;
        }
    }
}