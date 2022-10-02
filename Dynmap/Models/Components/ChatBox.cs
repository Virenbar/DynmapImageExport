using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    internal class ChatBox : Component
    {
        [JsonProperty("messagettl")]
        public long MessageTTL { get; set; }

        [JsonProperty("scrollback")]
        public long ScrollBack { get; set; }

        [JsonProperty("sendbutton")]
        public bool SendButton { get; set; }

        [JsonProperty("showplayerfaces")]
        public bool ShowPlayerFaces { get; set; }

        [JsonProperty("visiblelines")]
        public long VisibleLines { get; set; }
    }
}