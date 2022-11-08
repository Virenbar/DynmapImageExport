using Spectre.Console;
using System.Text.RegularExpressions;

namespace DynmapImageExport.Models
{
    /// <summary>
    /// Minecraft ingame point
    /// </summary>
    internal record Point(int X, int Y, int Z)
    {
        private static readonly Regex R = new(@"-?\d+");
        public static Point Parse(string point)
        {
            var Match = R.Matches(point);
            var S = Match.Select(x => int.Parse(x.Value)).ToList();
            if (S.Count == 3)
            {
                return new Point(S[0], S[1], S[2]);
            }
            throw new ArgumentException($"Invalid point: {point}", nameof(point));
        }
        public override string ToString() => $"[{X},{Y},{Z}]";
    }
}