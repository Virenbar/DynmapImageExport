using Newtonsoft.Json;

namespace Dynmap.Models
{
    public class Config
    {
        [JsonProperty("allowchat")]
        public bool AllowChat { get; set; }

        [JsonProperty("allowwebchat")]
        public bool AllowWebChat { get; set; }

        [JsonProperty("chatlengthlimit")]
        public int ChatLengthLimit { get; set; }

        [JsonProperty("components")]
        public List<Component> Components { get; set; }

        [JsonProperty("confighash")]
        public int ConfigHash { get; set; }

        [JsonProperty("coreversion")]
        public string CoreVersion { get; set; }

        [JsonProperty("cyrillic")]
        public bool Cyrillic { get; set; }

        [JsonProperty("defaultmap")]
        public string DefaultMap { get; set; }

        [JsonProperty("defaultworld")]
        public string DefaultWorld { get; set; }

        [JsonProperty("defaultzoom")]
        public int DefaultZoom { get; set; }

        [JsonProperty("dynmapversion")]
        public string DynmapVersion { get; set; }

        [JsonProperty("grayplayerswhenhidden")]
        public bool GrayPlayersWhenHidden { get; set; }

        [JsonProperty("joinmessage")]
        public string JoinMessage { get; set; }

        [JsonProperty("loggedin")]
        public bool LoggedIn { get; set; }

        [JsonProperty("login-enabled")]
        public bool LoginEnabled { get; set; }

        [JsonProperty("maxcount")]
        public int MaxCount { get; set; }

        [JsonProperty("msg-chatnotallowed")]
        public string MsgChatNotAllowed { get; set; }

        [JsonProperty("msg-chatrequireslogin")]
        public string MsgChatRequiresLogin { get; set; }

        [JsonProperty("msg-hiddennameoin")]
        public string MsgHiddenNameJoin { get; set; }

        [JsonProperty("msg-hiddennamequit")]
        public string MsgHiddenNameQuit { get; set; }

        [JsonProperty("msg-maptypes")]
        public string MsgMapTypes { get; set; }

        [JsonProperty("msg-players")]
        public string MsgPlayers { get; set; }

        [JsonProperty("quitmessage")]
        public string QuitMessage { get; set; }

        [JsonProperty("showlayercontrol")]
        public string ShowLayerControl { get; set; }

        [JsonProperty("showplayerfacesinmenu")]
        public bool ShowPLayerFacesInMenu { get; set; }

        [JsonProperty("sidebaropened")]
        public string SideBarOpened { get; set; }

        [JsonProperty("spammessage")]
        public string SpamMessage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("updaterate")]
        public double UpdateRate { get; set; }

        [JsonProperty("webchat-interval")]
        public double WebChatInterval { get; set; }

        [JsonProperty("webchat-requires-login")]
        public bool WebchatRequiresLogin { get; set; }

        [JsonProperty("webprefix")]
        public string WebPrefix { get; set; }

        [JsonProperty("worlds")]
        public List<World> Worlds { get; set; }
    }
}