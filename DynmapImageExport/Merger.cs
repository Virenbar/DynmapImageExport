using DynmapTools.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Point = SixLabors.ImageSharp.Point;

namespace DynmapTools
{
    internal class Merger
    {
        private const byte Size = 128;
        private readonly ImageMap Images;
        private readonly string Path;

        public Merger(ImageMap images, string path)
        {
            Images = images;
            Path = path;
        }

        public void Merge(IProgress<string> IP)
        {
            using var I = new Image<Rgba32>(Size * Images.Width, Size * Images.Height);
            var NImages = Images.Normilize();
            foreach (var (K, V) in NImages)
            {
                I.Mutate(O =>
                {
                    using var Tile = Image.Load(V);
                    O.DrawImage(Tile, new Point(K.DX * Size, K.DY * Size), 1);
                    IP.Report("");
                });
            }
            I.SaveAsPng(Path);
        }

        private void ff()
        {
            List<(int X, int Y)> T = Enumerable.Range(0, 100).SelectMany(X => Enumerable.Range(0, 100), (X, Y) => (X, Y)).ToList();

            using var I = new Image<Rgba32>(Size * 100, Size * 100);
            foreach (var (X, Y) in T)
            {
                I.Mutate(O =>
                {
                    using var II = Image.Load("I.png");
                    O.DrawImage(II, new Point(X * Size, Y * Size), 1);
                });
            }
            I.SaveAsPng("O.png");
        }
    }
}