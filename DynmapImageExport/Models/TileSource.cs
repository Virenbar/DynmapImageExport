using Dynmap;
using Dynmap.Models;

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

        public DynMap DynMap { get; }
        public Map Map { get; }
        public Uri TilesURL => DynMap.TilesURL;
        public string Title => DynMap.Config.Title.RemoveInvalidChars();
        public World World { get; }

        public Tile TileAtPoint(Point point, int zoom)
        {
            var (X, Y) = WTM(point, zoom);
            return new Tile(this, X, Y, zoom);
        }

        public string TilePath(int X, int Y, int zoom)
        {
            var prefix = zoom == 0 ? "" : $"{new string('z', zoom)}_";
            return $"{World.Name}/{Map.Name}/{X >> 5}_{Y >> 5}/{prefix}{X}_{Y}.{Map.ImageFormat ?? "png"}";
        }

        public string TileURL(int X, int Y, int zoom) => $"{DynMap.TilesURL}{TilePath(X, Y, zoom)}";

        private static int Round(double N, int Z) => (int)(Z * Math.Ceiling(N / Z));

        /// <summary>
        /// World to Map
        /// </summary>
        private (int X, int Y) WTM(Point point, int zoom)
        {
            var T = Map.WorldToMap;
            var lng = point.X * T[0] + point.Y * T[1] + point.Z * T[2];
            var lat = point.X * T[3] + point.Y * T[4] + point.Z * T[5];
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

            var Scale = (int)Math.Pow(2, zoom);
            var dimension = 128; //Can be different?

            // IDK How it works, but "it just works"
            // (Math.Ceiling((lng / dimension) / Z0) - 1) * Z0
            var X = Round(lng / dimension, Scale) - 1 * Scale;
            var Y = Round(-(128 - lat) / dimension, Scale);

            //-16 -16 = -1  0
            // 16 -16 =  0  0
            // 16  16 =  0 -1
            //-16  16 = -1  1

            return (X, Y);
        }
    }
}