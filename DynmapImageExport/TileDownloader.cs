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
        private readonly SemaphoreSlim Semaphore = new(4);
        private readonly string Title;

        public TileDownloader(TileMap range)
        {
            Range = range;
            Title = Range.Source.Title;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public async Task<ImageMap> Download(IProgress<string> IP = null)
        {
            Files.Clear();

            var T = Range.Select(async (KV) =>
            {
                var (K, V) = KV;
                var (B, Path) = await TryDownloadTile(V);
                if (B) { Files[K] = Path; }
                IP.Report("");
            });
            await Task.WhenAll(T);

            return Files;
        }

        private async Task<(bool, string)> TryDownloadTile(Tile tile)
        {
            var LocalPath = new FileInfo(Path.Combine("tiles", Title, tile.TilePath()));
            try
            {
                await Semaphore.WaitAsync();
                var C = await Client.GetAsync(tile.TileURL());
                Directory.CreateDirectory(LocalPath.DirectoryName);
                using var FS = new FileStream(LocalPath.FullName, FileMode.Create);
                await C.Content.CopyToAsync(FS);

                return (true, LocalPath.FullName);
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