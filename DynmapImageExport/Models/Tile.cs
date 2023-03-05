namespace DynmapImageExport.Models
{
    internal class Tile
    {
        internal Tile(TileSource source, int x, int y, int zoom)
        {
            Source = source;
            X = x;
            Y = y;
            Zoom = zoom;
        }

        public TileSource Source { get; }
        public int X { get; }
        public int Y { get; }
        public int Zoom { get; }

        public Tile Copy() => new(Source, X, Y, Zoom);

        public Tile NewTile(int x, int y) => new(Source, x, y, Zoom);

        public string TilePath() => Source.TilePath(X, Y, Zoom);

        public string TileURI() => Source.TileURI(X, Y, Zoom);

        public override string ToString() => $"[{X},{Y}]";
    }
}