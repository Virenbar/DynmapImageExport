using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class PlayerMarkers : Component
    {
        [JsonProperty("hidebydefault")]
        public bool HideByDefault { get; set; }

        [JsonProperty("showplayerhealth")]
        public bool ShowPlayerHealth { get; set; }

        [JsonProperty("showplayerbody")]
        public bool ShowPlayerBody { get; set; }

        [JsonProperty("showplayerfaces")]
        public bool ShowPlayerFaces { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("smallplayerfaces")]
        public bool SmallPlayerFaces { get; set; }

        [JsonProperty("layerprio")]
        public long Layerprio { get; set; }
    }
}