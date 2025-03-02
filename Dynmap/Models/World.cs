using Newtonsoft.Json;

namespace Dynmap.Models
{
    public class World
    {
        [JsonProperty("center")]
        public Point Center { get; set; }

        [JsonProperty("extrazoomout")]
        public int ExtraZoomOut { get; set; }

        [JsonProperty("maps")]
        public List<Map> Maps { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }

        [JsonProperty("sealevel")]
        public int SeaLevel { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("worldheight")]
        public int WorldHeight { get; set; }
    }
}