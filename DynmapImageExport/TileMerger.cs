using DynmapImageExport.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using Point = SixLabors.ImageSharp.Point;

namespace DynmapImageExport
{
    internal class TileMerger : IDisposable
    {
        private readonly ImageMap Images;
        private readonly Image Result;
        private readonly int Size;

        public TileMerger(ImageMap images)
        {
            Size = images.TileSize;
            Result = new Image<Rgba32>(Size * images.Width, Size * images.Height);
            Images = images;
        }

        public void Merge(IProgress<int> IP)
        {
            Trace.WriteLine($"Merge started: {Images.Count} images");
            var NImages = Images.Normalize();
            foreach (var (K, V) in NImages)
            {
                Result.Mutate(ctx =>
                {
                    using var Tile = Image.Load(V);
                    ctx.DrawImage(Tile, new Point(K.DX * Size, K.DY * Size), 1);
                    IP.Report(1);
                });
            }
            Trace.WriteLine($"Merge done: {Result.Width}px X {Result.Height}px");
        }

        public FileInfo Save(string path, ImageFormat format)
        {
            var File = format switch
            {
                ImageFormat.PNG => SavePNG(path),
                ImageFormat.JPG => SaveJPG(path),
                ImageFormat.WEBP => SaveWEBP(path),
                _ => throw new ArgumentException($"Invalid format: {format}", nameof(format))
            };
            Trace.WriteLine($"Image saved: {File.FullName}");
            return File;
        }

        public FileInfo Save(string path) => Save(path, ImageFormat.PNG);

        private FileInfo SaveJPG(string path)
        {
            Result.SaveAsJpeg(path + ".jpg");
            return new FileInfo(path);
        }

        private FileInfo SavePNG(string path)
        {
            Result.SaveAsPng(path + ".png");
            return new FileInfo(path);
        }

        private FileInfo SaveWEBP(string path)
        {
            Result.SaveAsWebp(path + ".webp");
            return new FileInfo(path);
        }

        #region IDisposable

        public void Dispose()
        {
            ((IDisposable)Result).Dispose();
        }

        #endregion IDisposable
    }
}