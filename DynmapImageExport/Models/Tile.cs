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

            TilePath = Source.TilePath(X, Y, Zoom);
            TileURI = Source.TileURI(X, Y, Zoom);
        }

        public TileSource Source { get; }
        public string TilePath { get; }
        public string TileURI { get; }
        public int X { get; }
        public int Y { get; }
        public int Zoom { get; }

        public Tile Copy() => new(Source, X, Y, Zoom);

        public Tile NewTile(int x, int y) => new(Source, x, y, Zoom);

        public override string ToString() => $"[{X},{Y}]";
    }
}