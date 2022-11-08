using System.Text.RegularExpressions;

namespace DynmapImageExport.Models
{
    internal record Padding(int Top, int Right, int Bottom, int Left)
    {
        private static readonly Regex R = new(@"\d+");
        public Padding(int All) : this(All, All, All, All) { }
        public Padding(int V, int H) : this(V, H, V, H) { }
        static public Padding Parse(string padding)
        {
            var Match = R.Matches(padding);
            var S = Match.Select(x => int.Parse(x.Value)).ToList();
            return S.Count switch
            {
                4 => new Padding(S[0], S[1], S[2], S[3]),
                2 => new Padding(S[0], S[1]),
                1 => new Padding(S[0]),
                _ => throw new ArgumentException($"Invalid padding: {padding}", nameof(padding))
            };
        }
        public int Width => Left + 1 + Right;
        public int Height => Top + 1 + Bottom;
        public override string ToString() => $"[{Top},{Right},{Bottom},{Left}]";
    }
}