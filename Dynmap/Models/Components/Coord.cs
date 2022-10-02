using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class Coord : Component
    {
        [JsonProperty("hidey")]
        public bool HideY { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("show-chunk")]
        public bool ShowChunk { get; set; }

        [JsonProperty("show-mcr")]
        public bool ShowMCR { get; set; }
    }
}