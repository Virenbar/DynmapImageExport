namespace DynmapTools.Models
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

        public string TilePath() => Source.TilePath(X, Y, Zoom);

        public string TileURL() => Source.TileURL(X, Y, Zoom);

        public TileSource Source { get; }
        public int X { get; }
        public int Y { get; }
        public int Zoom { get; }

        public override string ToString()
        {
            return $"{{X={X},Y={Y}}}";
        }

        public TileMap CreateTileRange(Padding range)
        {
            var Scale = 1 << Zoom;

            var Tiles = new TileMap(Source);

            for (int dy = -range.Top; dy <= range.Bottom; dy++)
            {
                for (int dx = -range.Left; dx <= range.Right; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        Tiles[(dx, dy)] = this;
                        continue;
                    }
                    var x = X + dx * Scale;
                    var y = Y - dy * Scale;
                    var tile = new Tile(Source, x, y, Zoom);
                    Tiles[(dx, dy)] = tile;
                }
            }
            return Tiles;
        }
    }
}