using DynmapImageExport.Arguments;
using DynmapImageExport.Extensions;
using DynmapImageExport.Models;
using DynmapImageExport.Options;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Padding = DynmapImageExport.Models.Padding;

namespace DynmapImageExport.Commands
{
    internal class MergeCommand : Command
    {
        private static readonly ProgressColumn[] Columns;
        private static readonly Stopwatch SW = new();

        static MergeCommand()
        {
            Columns = new ProgressColumn[]    {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn()
            };
        }

        public MergeCommand() : base("merge", "Merge dynmap to image")
        {
            AddAlias("m");
            AddArgument(new Argument<Uri>("url", "Dynmap URL"));
            AddArgument(new Argument<string>("world", "World name"));
            AddArgument(new Argument<string>("map", "Map name"));
            AddArgument(new PointArgument());
            AddOption(new PaddingOption());
            AddOption(new ZoomOption());
            AddOption(new OutputOption());
            AddOption(new FormatOption());
            AddOption(new NoCacheOption());

            Handler = CommandHandler.Create(HandleCommand);
        }

        private static async Task<int> HandleCommand(
            Uri URL,
            string world,
            string map,
            Point[] point,
            Padding padding,
            int? zoom,
            string output,
            ImageFormat? format,
            bool noCache)
        {
            AnsiConsole.MarkupLine($"[yellow]Merging of: {URL.Host} - {world} - {map}[/]");
            using var Dynmap = await Common.GetDynmap(URL);
            var World = Dynmap.GetWorld(world);
            var Map = World.GetMap(map);
            var Source = new TileSource(Dynmap, World, Map);
            Source.ValidateZoom(zoom);
            var Zoom = zoom ?? Source.ScaleToZoom(1);
            var Format = format ?? Source.ImageFormat;

            var PointTiles = point.Select(P => Source.PointToTile(P, Zoom)).ToList();
            var Tiles = TileMap.CreateTileMap(PointTiles, padding);

            var Points = string.Join("~", point.Select(P => $"{P}"));
            var Size = $"~{Tiles.Height} X ~{Tiles.Width}(~{Tiles.Height * 128}px X ~{Tiles.Width * 128}px)";
            var Info = new Grid()
                .AddColumns(2)
                .AddRow("Points:", Points.EscapeMarkup())
                .AddRow("Padding:", $"{padding}".EscapeMarkup())
                .AddRow("Tiles count:", $"~{Tiles.Count}")
                .AddRow("Image size:", Size);
            AnsiConsole.Write(Info);
            Trace.WriteLine($"Input: {world}-{map}-{Points}-{padding}-{Zoom}");

            var FilePath = output ?? $"{Source.Title} ({world}-{map}-{Points}-{padding}-{Zoom})";
            FilePath = Regex.Replace(FilePath, @"\.\w{3,4}$", "", RegexOptions.IgnoreCase);

            SW.Restart();

            var (Images, Merger) = await AnsiConsole.Progress()
                 .Columns(Columns)
                 .StartAsync(async ctx =>
                 {
                     var T = ctx.AddTask($"Downloading tiles: 0/{Tiles.Count}", true, Tiles.Count);
                     var T2 = ctx.AddTask($"Merging tiles: 0/0", false);
                     T2.IsIndeterminate = true;

                     using var TD = new TileDownloader(Tiles, 8) { UseCache = !noCache };
                     var Images = await TD.Download(T.AsProgress());

                     T2.MaxValue = Images.Count;
                     T2.IsIndeterminate = false;
                     T2.StartTask();
                     var TM = new TileMerger(Images);
                     TM.Merge(T2.AsProgress());
                     return (Images, TM);
                 });

            var Image = AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Saving image", ctx => Merger.Save(FilePath, Format));
            Merger.Dispose();

            SW.Stop();
            Size = $"{Images.Height} X {Images.Width}({Images.Height * 128}px X {Images.Width * 128}px)";
            var TP = new TextPath(Image.FullName)
                .LeafColor(Color.Yellow);
            var Out = new Grid()
                .AddColumns(2)
                .AddRow("[white]Merge done:[/]", $@"[yellow]{SW.Elapsed:hh\:mm\:ss}[/]")
                .AddRow("[white]Tiles count:[/]", $"{Images.Count}")
                .AddRow("[white]Image size:[/]", Size)
                .AddRow(new Markup("[white]Image path:[/]"), TP);
            AnsiConsole.Write(Out);
            return 0;
        }
    }
}