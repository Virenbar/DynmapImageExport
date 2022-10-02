using System.Collections.Concurrent;

namespace DynmapImageExport.Models
{
    internal class ImageMap : ConcurrentDictionary<(int DX, int DY), string>
    {
        public int MaxDX => Keys.Max(K => K.DX);
        public int MaxDY => Keys.Max(K => K.DY);
        public int MinDX => Keys.Min(K => K.DX);
        public int MinDY => Keys.Min(K => K.DY);

        #region W H
        public int Height => MaxDY - MinDY + 1;
        public int Width => MaxDX - MinDX + 1;
        #endregion W H

        /// <summary>
        /// Shift all deltas so they start from zero
        /// </summary>
        /// <returns>New ImageMap with normalized deltas</returns>
        public ImageMap Normilize()
        {
            ImageMap Normal = new();
            foreach (var (K, V) in this)
            {
                var Key = (K.DX - MinDX, K.DY - MinDY);
                Normal[Key] = V;
            }
            return Normal;
        }
    }
}