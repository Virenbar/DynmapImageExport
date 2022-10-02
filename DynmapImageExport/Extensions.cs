using DynmapImageExport.Options;
using System.CommandLine.Builder;

namespace DynmapImageExport
{
    public static class Extensions
    {
        public static string ToScale(this double scale) => scale >= 1 ? $"{scale}:1" : $"1:{1 / scale}";

        internal static CommandLineBuilder UseVerbose(this CommandLineBuilder builder) => Verbose.AddToBuilder(builder);
    }
}