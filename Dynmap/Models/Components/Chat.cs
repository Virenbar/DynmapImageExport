using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class Chat : Component
    {
        [JsonProperty("allowurlname")]
        public bool AllowURLName { get; set; }
    }
}