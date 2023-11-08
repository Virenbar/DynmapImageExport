using Dynmap.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dynmap
{
    public class DynMap : IDisposable
    {
        private readonly HttpClient Client;
        private readonly Uri URL;
        private URL URLs;

        public DynMap(string url) : this(new Uri(url)) { }

        public DynMap(Uri url)
        {
            URL = url;
            Client = new HttpClient { BaseAddress = URL };
            Trace.WriteLine($"Dynmap URL: {URL}");
        }

        public Config Config { get; private set; }

        public Dictionary<(string world, string map), Map> Maps { get; private set; }
        public Uri TilesURL => new(URL, URLs.Tiles);
        public Dictionary<string, World> Worlds { get; private set; }

        public async Task RefreshConfig()
        {
            var configURL = "standalone/config.js";
            Trace.WriteLine($"Fetching: {configURL}");
            var URL_JS = await Client.GetStringAsync(configURL);
            var Match = Regex.Match(URL_JS, @"url\s*:\s*(.+)};", RegexOptions.Singleline);
            URLs = JsonConvert.DeserializeObject<URL>(Match.Groups[1].Value);

            var ConfigFile = ApplyTimestamp(URLs.Configuration);
            Trace.WriteLine($"Fetching: {ConfigFile}");
            var config = await Client.GetStringAsync(ConfigFile);
            Config = JsonConvert.DeserializeObject<Config>(config);

            Worlds = Config.Worlds.ToDictionary(W => W.Name, W => W);
            Maps = Config.Worlds
                .SelectMany(W => W.Maps.Select(M => (W, M)))
                .ToDictionary(X => (X.W.Name, X.M.Name), X => X.M);
        }

        private static string ApplyTimestamp(string str) => str.Replace("{timestamp}", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());

        #region Disposable

        public void Dispose() => ((IDisposable)Client).Dispose();

        #endregion Disposable
    }
}