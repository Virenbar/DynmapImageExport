using System.Collections.Concurrent;

namespace DynmapTools.Models
{
    internal class ImageMap : ConcurrentDictionary<(int DX, int DY), string>
    {
        public int Height => MaxDY - MinDY + 1;
        public int MaxDX => Keys.Max(K => K.DX);
        public int MaxDY => Keys.Max(K => K.DY);
        public int MinDX => Keys.Min(K => K.DX);
        public int MinDY => Keys.Min(K => K.DY);
        public int Width => MaxDX - MinDX + 1;

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