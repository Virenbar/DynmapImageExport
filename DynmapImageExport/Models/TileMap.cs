namespace DynmapImageExport.Models
{
    internal class TileMap : Dictionary<(int DX, int DY), Tile>
    {
        public TileMap(TileSource source)
        {
            Source = source;
        }

        #region Properties
        public int Height => MaxDY - MinDY + 1;
        public int MaxDX => Keys.Max(T => T.DX);
        public int MaxDY => Keys.Max(T => T.DY);
        public int MinDX => Keys.Min(T => T.DX);
        public int MinDY => Keys.Min(T => T.DY);
        public int Width => MaxDX - MinDX + 1;
        #endregion Properties

        public TileSource Source { get; }

        public static TileMap CreateTileMap(IEnumerable<Tile> tiles, Padding padding)
        {
            var MinX = tiles.Min(T => T.X);
            var MaxX = tiles.Max(T => T.X);
            var MinY = tiles.Min(T => T.Y);
            var MaxY = tiles.Max(T => T.Y);

            //var TileOrigin = tiles.OrderBy(T => T.Y).OrderBy(T => T.X).First();
            // Top left tile
            var TileOrigin = tiles.First().NewTile(MinX, MaxY);

            var Scale = 1 << TileOrigin.Zoom;
            var Tiles = new TileMap(TileOrigin.Source)
            {
                [(0, 0)] = TileOrigin
            };
            foreach (var tile in tiles)
            {
                var dx = (tile.X - TileOrigin.X) / Scale;
                var dy = -((tile.Y - TileOrigin.Y) / Scale);
                if (Tiles.ContainsKey((dx, dy))) { continue; }
                Tiles[(dx, dy)] = tile;
            }
            var Width = Tiles.Width;
            var Height = Tiles.Height;
            for (int dy = -padding.Top; dy < Height + padding.Bottom; dy++)
            {
                for (int dx = -padding.Left; dx < Width + padding.Right; dx++)
                {
                    if (Tiles.ContainsKey((dx, dy))) { continue; }
                    var x = TileOrigin.X + dx * Scale;
                    var y = TileOrigin.Y - dy * Scale;
                    Tiles[(dx, dy)] = TileOrigin.NewTile(x, y);
                }
            }
            return Tiles;
        }
    }
}