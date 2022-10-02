namespace DynmapTools.Models
{
    internal class TileMap : Dictionary<(int DX, int DY), Tile>
    {
        public TileMap(TileSource source)
        {
            Source = source;
        }

        public TileSource Source { get; }
    }
}