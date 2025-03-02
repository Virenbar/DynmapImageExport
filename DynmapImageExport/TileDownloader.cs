using DynmapImageExport.Models;
using System.Diagnostics;
using System.Net;

namespace DynmapImageExport
{
    internal class TileDownloader : IDisposable
    {
        private readonly HttpClient Client;
        private readonly SemaphoreSlim Semaphore;
        private readonly TileMap Tiles;
        private readonly string Title;

        /// <summary>
        ///
        /// </summary>
        /// <param name="tiles">Tiles to download</param>
        /// <param name="threads">Number of threads</param>
        public TileDownloader(TileMap tiles, int threads)
        {
            // TilesURI can have URLSearchParams so BaseAddress doesn't work
            Client = new HttpClient(new HttpClientHandler { MaxConnectionsPerServer = threads });
            Tiles = tiles;
            Title = Tiles.Source.Title;
            Semaphore = new(threads);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public bool UseCache { get; set; } = true;

        /// <summary>
        /// Downloads tiles to local storage and returns <see cref="ImageMap"/>
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public async Task<ImageMap> Download(IProgress<int> IP = default)
        {
            Trace.WriteLine($"Download started: {Tiles.Count} tiles");
            var tileSize = 128 << Tiles.Source.Map.TileScale;
            ImageMap Files = new(tileSize);
            var Tasks = Tiles.Select(async (KV) =>
            {
                var (DXY, Tile) = KV;
                if (await TryDownloadTile(Tile) is string path) { Files[DXY] = path; }
                IP.Report(1);
            });
            await Task.WhenAll(Tasks);

            Trace.WriteLine($"Download done: {Files.Count} tiles");
            return Files;
        }

        private async Task<string> TryDownloadTile(Tile tile)
        {
            var LocalFile = new FileInfo(Path.Combine("tiles", Title, tile.TilePath));
            if (UseCache && LocalFile.Exists)
            {
                Trace.WriteLine($"Cached tile: {tile.TilePath} ");
                return LocalFile.FullName;
            }
            try
            {
                await Semaphore.WaitAsync();
                Trace.WriteLine($"Downloading tile: {tile.TileURI} ");
                using var Responce = await Client.GetAsync(tile.TileURI, HttpCompletionOption.ResponseHeadersRead);
                if (!Responce.IsSuccessStatusCode)
                {
                    Trace.WriteLine($"Tile: {tile} - {Responce.StatusCode}({Responce.ReasonPhrase})");
                    return null;
                }

                Directory.CreateDirectory(LocalFile.DirectoryName);
                using var FS = new FileStream(LocalFile.FullName, FileMode.Create);
                using var RS = await Responce.Content.ReadAsStreamAsync();
                await RS.CopyToAsync(FS);

                return LocalFile.FullName;
            }
            catch (HttpRequestException e)
            {
                Trace.WriteLine($"Tile: {tile} - {e.Message}");
            }
            finally
            {
                Semaphore.Release();
            }
            return null;
        }

        #region IDispose

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
        }

        #endregion IDispose
    }
}