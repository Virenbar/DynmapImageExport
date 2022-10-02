using Newtonsoft.Json;

namespace Dynmap.Models.Components
{
    public class TimeOfDayClock : Component
    {
        [JsonProperty("showdigitalclock")]
        public bool ShowDigitalClock { get; set; }

        [JsonProperty("showweather")]
        public bool ShowWeather { get; set; }
    }
}