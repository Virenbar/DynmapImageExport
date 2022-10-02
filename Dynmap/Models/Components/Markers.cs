using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class Markers : Component
    {
        [JsonProperty("default-sign-set")]
        public string DefaultSignSet { get; set; }

        [JsonProperty("enablesigns")]
        public bool EnableSigns { get; set; }

        [JsonProperty("maxofflinetime")]
        public long MaxOfflineTime { get; set; }

        [JsonProperty("offlinehidebydefault")]
        public bool OfflineHideByDefault { get; set; }

        [JsonProperty("offlineicon")]
        public string OfflineIcon { get; set; }

        [JsonProperty("offlinelabel")]
        public string OfflineLabel { get; set; }

        [JsonProperty("offlineminzoom")]
        public long OfflineMinZoom { get; set; }

        [JsonProperty("showlabel")]
        public bool ShowLabel { get; set; }

        [JsonProperty("showofflineplayers")]
        public bool ShowOfflinePlayers { get; set; }

        [JsonProperty("showspawn")]
        public bool ShowSpawn { get; set; }

        [JsonProperty("showspawnbeds")]
        public bool ShowSpawnBeds { get; set; }

        [JsonProperty("showworldborder")]
        public bool ShowWorldBorder { get; set; }

        [JsonProperty("spawnbedformat")]
        public string SpawnBedFormat { get; set; }

        [JsonProperty("spawnbedhidebydefault")]
        public bool SpawnBedHideByDefault { get; set; }

        [JsonProperty("spawnbedicon")]
        public string SpawnBedIcon { get; set; }

        [JsonProperty("spawnbedlabel")]
        public string SpawnBedLabel { get; set; }

        [JsonProperty("spawnbedminzoom")]
        public long SpawnBedMinZoom { get; set; }

        [JsonProperty("spawnicon")]
        public string SpawnIcon { get; set; }

        [JsonProperty("spawnlabel")]
        public string SpawnLabel { get; set; }

        [JsonProperty("worldborderlabel")]
        public string WorldBorderLabel { get; set; }
    }
}