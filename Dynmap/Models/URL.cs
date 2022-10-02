using Newtonsoft.Json;

namespace Dynmap.Models
{
    public class URL
    {
        [JsonProperty("configuration")]
        public string Configuration { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("markers")]
        public string Markers { get; set; }

        [JsonProperty("register")]
        public string Register { get; set; }

        [JsonProperty("sendmessage")]
        public string SendMessage { get; set; }

        [JsonProperty("tiles")]
        public string Tiles { get; set; }

        [JsonProperty("update")]
        public string Update { get; set; }
    }
}