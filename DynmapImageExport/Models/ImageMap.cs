using System.Collections.Concurrent;

namespace DynmapImageExport.Models
{
    /// <summary>
    /// Dictionary of URLs to tiles
    /// </summary>
    internal class ImageMap : ConcurrentDictionary<(int DX, int DY), string>
    {
        public ImageMap(int tileSize)
        {
            TileSize = tileSize;
        }

        /// <summary>
        /// Size of each image in pixels
        /// </summary>
        public int TileSize { get; }

        private int MaxDX => Keys.Max(K => K.DX);
        private int MaxDY => Keys.Max(K => K.DY);
        private int MinDX => Keys.Min(K => K.DX);
        private int MinDY => Keys.Min(K => K.DY);

        #region W H
        public int Height => MaxDY - MinDY + 1;
        public int Width => MaxDX - MinDX + 1;
        #endregion W H

        /// <summary>
        /// Shift all deltas so they start from zero
        /// </summary>
        /// <returns>New ImageMap with normalized deltas</returns>
        public ImageMap Normalize()
        {
            var minDX = MinDX;
            var minDY = MinDY;
            ImageMap Normal = new(TileSize);
            foreach (var (K, V) in this)
            {
                var Key = (K.DX - minDX, K.DY - minDY);
                Normal[Key] = V;
            }
            return Normal;
        }
    }
}