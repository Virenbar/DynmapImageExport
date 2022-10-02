using Dynmap.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dynmap
{
    public class DynMap
    {
        private readonly string URL;
        private URL URLs;

        public DynMap(string url)
        {
            URL = url.TrimEnd('/');
            Trace.WriteLine($"Dynmap URL: {URL}");
        }

        public Config Config { get; private set; }

        public Dictionary<(string world, string map), Map> Maps { get; private set; }

        public string TilesURL => $"{URL}/{URLs.Tiles}";
        public Dictionary<string, World> Worlds { get; private set; }

        public async Task RefreshConfig()
        {
            using var Client = new HttpClient();
            var URLFile = $"{URL}/standalone/config.js";

            Trace.WriteLine($"Fetching: {URLFile}");
            var urls = await Client.GetStringAsync(URLFile);
            var Match = Regex.Match(urls, "url : (.+)};", RegexOptions.Singleline);
            URLs = JsonConvert.DeserializeObject<URL>(Match.Groups[1].Value);

            var ConfigFile = $"{URL}/{ApplyTimestamp(URLs.Configuration)}";
            Trace.WriteLine($"Fetching: {ConfigFile}");
            var config = await Client.GetStringAsync(ConfigFile);
            Config = JsonConvert.DeserializeObject<Config>(config);

            Worlds = Config.Worlds.ToDictionary(W => W.Name, W => W);
            Maps = Config.Worlds
                .SelectMany(W => W.Maps.Select(M => (W, M)))
                .ToDictionary(X => (X.W.Name, X.M.Name), X => X.M);
        }

        public string Tiles(string world) => URLs?.Tiles.Replace("{world}", world);

        private static string ApplyTimestamp(string str) => str.Replace("{timestamp}", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());
    }
}