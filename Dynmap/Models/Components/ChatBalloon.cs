using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class ChatBalloon : Component
    {
        [JsonProperty("focuschatballoons")]
        public bool FocusChatBalloons { get; set; }
    }
}