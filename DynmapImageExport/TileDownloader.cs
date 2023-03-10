using System.Diagnostics;
using System.Net;
using DynmapImageExport.Models;

namespace DynmapImageExport
{
    internal class TileDownloader : IDisposable
    {
        private readonly ImageMap Files = new();
        private readonly SemaphoreSlim Semaphore;
        private readonly TileMap Tiles;
        private readonly string Title;
        private HttpClient Client;

        public TileDownloader(TileMap tiles) : this(tiles, 4) { }

        public TileDownloader(TileMap tiles, int threads)
        {
            Tiles = tiles;
            Title = Tiles.Source.Title;
            Semaphore = new(threads);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public bool UseCache { get; set; } = true;

        public async Task<ImageMap> Download(IProgress<int> IP = default)
        {
            Trace.WriteLine($"Download started: {Tiles.Count} tiles");
            using (Client = new() { BaseAddress = Tiles.Source.TilesURI })
            {
                Files.Clear();
                var Tasks = Tiles.Select(async (KV) =>
                {
                    var (DXY, Tile) = KV;
                    if (await TryDownloadTile(Tile) is string path) { Files[DXY] = path; }
                    IP.Report(1);
                });
                await Task.WhenAll(Tasks);
            }
            Trace.WriteLine($"Download done: {Files.Count} tiles");
            return Files;
        }

        private async Task<string> TryDownloadTile(Tile tile)
        {
            var LocalFile = new FileInfo(Path.Combine("tiles", Title, tile.TilePath()));
            if (UseCache && LocalFile.Exists)
            {
                Trace.WriteLine($"Cached tile: {tile.TilePath()} ");
                return LocalFile.FullName;
            }
            try
            {
                await Semaphore.WaitAsync();
                var URL = Tiles.Source.TilesURI + tile.TilePath();
                Trace.WriteLine($"Downloading tile: {URL} ");
                var Responce = await Client.GetAsync(URL);
                if (!Responce.IsSuccessStatusCode)
                {
                    Trace.WriteLine($"Tile: {tile} - {Responce.StatusCode}({Responce.ReasonPhrase})");
                    return null;
                }

                Directory.CreateDirectory(LocalFile.DirectoryName);
                using var FS = new FileStream(LocalFile.FullName, FileMode.Create);
                await Responce.Content.CopyToAsync(FS);

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