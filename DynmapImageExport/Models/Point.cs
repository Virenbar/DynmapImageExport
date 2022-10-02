namespace DynmapImageExport.Models
{
    /// <summary>
    /// Minecraft ingame point
    /// </summary>
    internal record Point(int X, int Y, int Z)
    {
        public static Point Parse(string point)
        {
            var S = point.Split(new[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (S.Count != 3) { throw new ArgumentException($"Invalid point argument: {point}", nameof(point)); }
            return new Point(S[0], S[1], S[2]);
        }
        public override string ToString() => $"{{X={X},Y={Y},Z={Z}}}";
    }
}