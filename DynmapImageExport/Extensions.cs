using DynmapTools.Options;
using System.CommandLine.Builder;

namespace DynmapTools
{
    public static class Extensions
    {
        public static string ToScale(this double scale) => scale >= 1 ? $"{scale}:1" : $"1:{1 / scale}";

        internal static CommandLineBuilder UseVerbose(this CommandLineBuilder builder) => Verbose.AddToBuilder(builder);
    }
}