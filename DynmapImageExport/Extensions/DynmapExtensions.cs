using Dynmap;
using Dynmap.Models;

namespace DynmapImageExport.Extensions
{
    internal static class DynmapExtensions
    {
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

        internal static string ScaleToString(this Map _, double scale) => scale >= 1 ? $"{scale}:1" : $"1:{1 / scale}";

        internal static double ZoomToScale(this Map map, int zoom) => map.Scale / Math.Pow(2, zoom);

        internal static string ZoomToString(this Map map, int zoom) => map.ScaleToString(map.ZoomToScale(zoom));
    }
}