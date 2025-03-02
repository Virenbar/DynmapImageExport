﻿using DynmapImageExport.Arguments;
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
            AddOption(new ThreadsOption());

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
            bool noCache,
            int? threads)
        {
            AnsiConsole.MarkupLine($"[yellow]Merging of: {URL.Host} - {world} - {map}[/]");
            using var Dynmap = await Common.GetDynmap(URL);
            var Source = new TileSource(Dynmap, world, map);
            Source.ValidateZoom(zoom);
            var Point = point ?? new[] { Source.World.Center.ToPoint() };
            var Zoom = zoom ?? Source.Map.ScaleToZoom(1);
            var Format = format ?? Source.Map.GetImageFormat();

            var PointTiles = Point.Select(P => Source.PointToTile(P, Zoom)).ToList();
            var Tiles = TileMap.CreateTileMap(PointTiles, padding);

            var Points = string.Join("~", Point.Select(P => $"{P}"));
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
            var (Images, Image) = await AnsiConsole.Progress()
                 .Columns(Columns)
                 .StartAsync(async ctx =>
                 {
                     var T = ctx.AddTask($"Downloading tiles: 0/{Tiles.Count}", true, Tiles.Count);
                     using var TD = new TileDownloader(Tiles, threads ?? 4) { UseCache = !noCache };
                     var Images = await TD.Download(T.AsProgress());

                     var T2 = ctx.AddTask($"Merging tiles: 0/{Images.Count}", true, Images.Count);
                     using var TM = new TileMerger(Images);
                     TM.Merge(T2.AsProgress());

                     var T3 = ctx.AddTask("Saving image", true, 1);
                     T3.IsIndeterminate = true;
                     var Image = TM.Save(FilePath, Format);
                     T3.Increment(1);

                     return (Images, Image);
                 });

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