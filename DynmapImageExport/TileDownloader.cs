using System.Diagnostics;
using System.Net;
using DynmapImageExport.Models;

namespace DynmapImageExport
{
    internal class TileDownloader : IDisposable
    {
        private readonly HttpClient Client = new();
        private readonly ImageMap Files = new();
        private readonly TileMap Range;
        private readonly SemaphoreSlim Semaphore;
        private readonly string Title;
        public bool UseCache { get; set; } = true;

        public TileDownloader(TileMap range) : this(range, 4) { }

        public TileDownloader(TileMap range, int threads)
        {
            Range = range;
            Title = Range.Source.Title;
            Semaphore = new(threads);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public async Task<ImageMap> Download(IProgress<string> IP = null)
        {
            Trace.WriteLine($"Download started: {Range.Count} tiles");
            Trace.Indent();
            Files.Clear();
            var Tasks = Range.Select(async (KV) =>
            {
                var (K, V) = KV;
                var (B, Path) = await TryDownloadTile(V);
                if (B) { Files[K] = Path; }
                IP.Report("");
            });
            await Task.WhenAll(Tasks);
            Trace.Unindent();
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
                var URL = tile.TileURL();
                Trace.WriteLine($"Downloading tile: {URL} ");
                var C = await Client.GetAsync(URL);
                Directory.CreateDirectory(LocalFile.DirectoryName);
                using var FS = new FileStream(LocalFile.FullName, FileMode.Create);
                await C.Content.CopyToAsync(FS);

                return (true, LocalFile.FullName);
            }
            catch (WebException e)
            {
                Trace.WriteLine($"Download error: {tile} - {e.Message}");
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