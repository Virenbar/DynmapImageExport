namespace DynmapTools.Models
{
    internal record Padding(int Top, int Right, int Bottom, int Left)
    {
        public Padding(int All) : this(All, All, All, All) { }
        public Padding(int V, int H) : this(V, H, V, H) { }
        static public Padding Parse(string range)
        {
            var S = range.Split(new[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            return S.Count switch
            {
                4 => new Padding(S[0], S[1], S[2], S[3]),
                2 => new Padding(S[0], S[1]),
                1 => new Padding(S[0]),
                _ => throw new ArgumentException($"Invalid range argument: {range}", nameof(range))
            };
        }
        public int Width => Left + 1 + Right;
        public int Height => Top + 1 + Bottom;
        public override string ToString() => $"{{Top={Top},Right={Right},Bottom={Bottom},Left={Left}}}";
    }
}