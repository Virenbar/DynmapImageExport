using System.Diagnostics;
using System.Net;
using DynmapImageExport.Models;

namespace DynmapImageExport
{
    internal class TileDownloader : IDisposable
    {
        private readonly ImageMap Files = new();
        private readonly TileMap Range;
        private readonly SemaphoreSlim Semaphore;
        private readonly string Title;
        private HttpClient Client;

        public TileDownloader(TileMap range) : this(range, 4) { }

        public TileDownloader(TileMap range, int threads)
        {
            Range = range;
            Title = Range.Source.Title;
            Semaphore = new(threads);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public bool UseCache { get; set; } = true;

        public async Task<ImageMap> Download(IProgress<int> IP = null)
        {
            Trace.WriteLine($"Download started: {Range.Count} tiles");
            using (Client = new() { BaseAddress = Range.Source.TilesURL })
            {
                Files.Clear();
                var Tasks = Range.Select(async (KV) =>
                {
                    var (K, V) = KV;
                    var (B, Path) = await TryDownloadTile(V);
                    if (B) { Files[K] = Path; }
                    IP?.Report(1);
                });
                await Task.WhenAll(Tasks);
            }
            Trace.WriteLine($"Download done: {Files.Count} tiles");
            return Files;
        }

        private async Task<(bool, string)> TryDownloadTile(Tile tile)
        {
            var LocalFile = new FileInfo(Path.Combine("tiles", Title, tile.TilePath()));
            if (UseCache && LocalFile.Exists)
            {
                Trace.WriteLine($"Cached tile: {tile.TileURL()} ");
                return (true, LocalFile.FullName);
            }
            try
            {
                await Semaphore.WaitAsync();
                var URL = tile.TilePath();
                Trace.WriteLine($"Downloading tile: {URL} ");
                var Responce = await Client.GetAsync(URL);
                if (!Responce.IsSuccessStatusCode)
                {
                    Trace.WriteLine($"Tile not found: {tile}");
                    return (false, null);
                }

                Directory.CreateDirectory(LocalFile.DirectoryName);
                using var FS = new FileStream(LocalFile.FullName, FileMode.Create);
                await Responce.Content.CopyToAsync(FS);

                return (true, LocalFile.FullName);
            }
            catch (WebException e)
            {
                Trace.WriteLine($"Tile: {tile} - {e.Message}");
            }
            finally
            {
                Semaphore.Release();
            }
            return (false, null);
        }

        #region IDispose

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
        }

        #endregion IDispose
    }
}