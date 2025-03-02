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

        internal TileSource(DynMap dynmap, string world, string map)
        {
            DynMap = dynmap;
            World = DynMap.GetWorld(world);
            Map = World.GetMap(map);
        }

        #region Dynmap
        public DynMap DynMap { get; }
        public Map Map { get; }
        public World World { get; }
        #endregion Dynmap

        public Uri TilesURI => DynMap.TilesURL;

        public string Title => DynMap.Config.Title.RemoveInvalidChars();

        public Tile PointToTile(Point point, int zoom)
        {
            var WTM = Map.WorldToMap;
            var lng = point.X * WTM[0] + point.Y * WTM[1] + point.Z * WTM[2];
            var lat = point.X * WTM[3] + point.Y * WTM[4] + point.Z * WTM[5];

            var zoomScale = 1 << zoom;
            var tileSize = 128 << Map.TileScale;
            /* Examples
            192 88 192 Z=0
            https://map.minecrafting.ru/tiles/world/flat/0_-1/5_-7.png

            192 88 192 Z=2
            https://map.minecrafting.ru/tiles/world/flat/0_-1/zz_4_-4.png
            */
            /* Invalid
            // https://github.com/webbukkit/dynmap/blob/v3.0/DynmapCore/src/main/resources/extracted/web/js/hdmap.js#L1
            // https://github.com/webbukkit/dynmap/blob/v3.0/DynmapCore/src/main/resources/extracted/web/js/dynmaputils.js#L245

            var z = 1 << Map.MapZoomOut;
            var X0 = lng / z;
            var Y0 = -((tileSize - lat) / z);
            X0 = X0 * zoomScale;
            Y0 = Y0 * zoomScale;

            var X1 = (lng / tileSize) - 1 * zoomScale;
            var Y1 = (-(128 - lat) / tileSize);
            */

            // IDK How it works, but "it just works"
            // (Math.Ceiling((lng / dimension) / Z0) - 1) * Z0
            var X = Round(lng / tileSize, zoomScale) - 1 * zoomScale;
            var Y = Round(-(128 - lat) / tileSize, zoomScale);
            /*
            -16 -16 = -1  0
             16 -16 =  0  0
             16  16 =  0 -1
            -16  16 = -1  1
            */
            return new Tile(this, X, Y, zoom);
        }

        public string TilePath(int X, int Y, int zoom)
        {
            var prefix = zoom == 0 ? "" : $"{new string('z', zoom)}_";
            return $"{World.Name}/{Map.Prefix}/{X >> 5}_{Y >> 5}/{prefix}{X}_{Y}.{Map.ImageFormat ?? "png"}";
        }

        public string TileURI(int X, int Y, int zoom) => $"{TilesURI}{TilePath(X, Y, zoom)}";

        /// <summary>
        /// Checks if map supports provided zoom
        /// </summary>
        /// <param name="zoom">Zoom to check</param>
        /// <exception cref="ArgumentException"></exception>
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

        private static int Round(double N, int Z) => (int)(Z * Math.Ceiling(N / Z));
    }
}