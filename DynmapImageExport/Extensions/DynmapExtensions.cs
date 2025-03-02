using Dynmap;
using Dynmap.Models;
using DynmapImageExport.Models;

namespace DynmapImageExport.Extensions
{
    internal static class DynmapExtensions
    {
        internal static ImageFormat GetImageFormat(this Map map)
        {
            return map.ImageFormat?.ToLowerInvariant() switch
            {
                "png" => ImageFormat.PNG,
                "jpg" => ImageFormat.JPG,
                "webp" => ImageFormat.WEBP,
                _ => ImageFormat.PNG
            };
        }

        internal static Map GetMap(this World world, string map)
        {
            var Map = world.Maps.Find(M => M.Name.ToLowerInvariant() == map.ToLowerInvariant());
            if (Map is null)
            {
                throw new ArgumentException($"Invalid map name: {map}");
            }
            return Map;
        }

        internal static Map GetMap(this DynMap dynmap, string world, string map)
        {
            var World = dynmap.GetWorld(world);
            var Map = World.Maps.Find(M => M.Name.ToLowerInvariant() == map.ToLowerInvariant());
            if (Map is null)
            {
                throw new ArgumentException($"Invalid map name: {map}");
            }
            return Map;
        }

        internal static World GetWorld(this DynMap dynmap, string world)
        {
            var World = dynmap.Config.Worlds.Find(W => W.Name.ToLowerInvariant() == world.ToLowerInvariant());
            if (World is null)
            {
                throw new ArgumentException($"Invalid world name: {world}");
            }
            return World;
        }

        internal static int ScaleToZoom(this Map map, double scale) => (int)Math.Log(map.Scale / scale, 2);

        internal static Models.Point ToPoint(this Dynmap.Models.Point point) => new((int)point.X, (int)point.Y, (int)point.Z);

        internal static double ZoomToScale(this Map map, int zoom) => map.Scale / Math.Pow(2, zoom);

        internal static string ZoomToString(this Map map, int zoom) => ScaleToString(map.ZoomToScale(zoom));

        private static string ScaleToString(double scale) => scale >= 1 ? $"{scale}:1" : $"1:{1 / scale}";
    }
}