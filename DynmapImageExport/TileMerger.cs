using DynmapImageExport.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using Point = SixLabors.ImageSharp.Point;

namespace DynmapImageExport
{
    internal class TileMerger
    {
        private const byte Size = 128;
        private readonly ImageMap Images;
        private readonly string Path;

        public TileMerger(ImageMap images, string path)
        {
            Images = images;
            Path = path;
        }

        public void Merge(IProgress<string> IP)
        {
            Trace.WriteLine($"Merge started: {Images.Count} images");
            using var Result = new Image<Rgba32>(Size * Images.Width, Size * Images.Height);
            var NImages = Images.Normilize();
            foreach (var (K, V) in NImages)
            {
                Result.Mutate(O =>
                {
                    using var Tile = Image.Load(V);
                    O.DrawImage(Tile, new Point(K.DX * Size, K.DY * Size), 1);
                    IP.Report("");
                });
            }
            Result.SaveAsPng(Path);
            Trace.WriteLine($"Merge done: {Result.Width}px X {Result.Height}px");
        }
    }
}