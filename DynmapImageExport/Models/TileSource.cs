using Dynmap;
using Dynmap.Models;
using DynmapImageExport.Extensions;

namespace DynmapImageExport.Models
{
    internal class TileSource
    {
        internal TileSource(DynMap dynmap, World world, Map map)
        {
            DynMap = dynmap;
            World = world;
            Map = map;
        }

        #region Dynmap
        public DynMap DynMap { get; }
        public Map Map { get; }
        public World World { get; }
        #endregion Dynmap

        public ImageFormat ImageFormat => Map.ImageFormat?.ToLowerInvariant() switch
        {
            "png" => ImageFormat.PNG,
            "jpg" => ImageFormat.JPG,
            "webp" => ImageFormat.WEBP,
            _ => ImageFormat.PNG
        };

        public Uri TilesURI => DynMap.TilesURL;

        public string Title => DynMap.Config.Title.RemoveInvalidChars();

        public static string ScaleToString(double scale) => scale >= 1 ? $"{scale}:1" : $"1:{1 / scale}";

        public Tile PointToTile(Point point, int zoom)
        {
            var WTM = Map.WorldToMap;
            var lng = point.X * WTM[0] + point.Y * WTM[1] + point.Z * WTM[2];
            var lat = point.X * WTM[3] + point.Y * WTM[4] + point.Z * WTM[5];
            /* Examples
            -70 64 70 Z=0
            https://map.minecrafting.ru/tiles/world/flat/-1_0/-3_2.png

            -70 64 -70 Z=2
            https://map.minecrafting.ru/tiles/world/flat/-1_0/zz_-4_4.png
            */
            /* Invalid
            var X = (int)lng / (1 << Map.MapZoomOut) - 1;
            var Y = (int)-((128 - lat) / (1 << Map.MapZoomOut));

            var zz = Math.Max(0, zoom - Map.MapZoomIn);
            var Scale = 1 << zz;
            var Sx = Scale * X;
            var Sy = Scale * Y;

            var XX = Sx >> 5;
            var YY = Sy >> 5;
            */

            // Scale = (int)Math.Pow(2, zoom);
            var ZoomScale = 1 << zoom;
            var dimension = 128; // Can be different?

            // IDK How it works, but "it just works"
            // (Math.Ceiling((lng / dimension) / Z0) - 1) * Z0
            var X = Round(lng / dimension, ZoomScale) - 1 * ZoomScale;
            var Y = Round(-(128 - lat) / dimension, ZoomScale);
            /*
            -16 -16 = -1  0
             16 -16 =  0  0
             16  16 =  0 -1
            -16  16 = -1  1
            */
            return new Tile(this, X, Y, zoom);
        }

        public int ScaleToZoom(double scale) => (int)Math.Log(Map.Scale / scale, 2);

        public string TilePath(int X, int Y, int zoom)
        {
            var prefix = zoom == 0 ? "" : $"{new string('z', zoom)}_";
            return $"{World.Name}/{Map.Prefix}/{X >> 5}_{Y >> 5}/{prefix}{X}_{Y}.{Map.ImageFormat ?? "png"}";
        }

        public string TileURI(int X, int Y, int zoom) => $"{TilesURI}{TilePath(X, Y, zoom)}";

        public void ValidateZoom(int? zoom)
        {
            if (zoom is null) { return; }
            if (zoom < 0)
            {
                throw new ArgumentException($"Invalid zoom: {zoom} (Minimum zoom: 0)");
            }
            if (zoom > Map.MapZoomOut)
            {
                throw new ArgumentException($"Invalid zoom: {zoom} (Maximum zoom: {Map.MapZoomOut})");
            }
        }

        public double ZoomToScale(int zoom) => Map.Scale / (1 << zoom);

        public string ZoomToString(int zoom) => ScaleToString(ZoomToScale(zoom));

        private static int Round(double N, int Z) => (int)(Z * Math.Ceiling(N / Z));
    }
}