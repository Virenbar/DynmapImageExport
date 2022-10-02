using Newtonsoft.Json;

namespace Dynmap.Models
{
    [JsonConverter(typeof(ComponentConverter))]
    public class Component
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}