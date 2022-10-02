using Newtonsoft.Json;

namespace Dynmap.Models
{
    public class Map
    {
        [JsonProperty("azimuth")]
        public double Azimuth { get; set; }

        [JsonProperty("background")]
        public object Background { get; set; }

        [JsonProperty("backgroundday")]
        public object BackgroundDay { get; set; }

        [JsonProperty("backgroundnight")]
        public object BackgroundNight { get; set; }

        [JsonProperty("bigmap")]
        public bool BigMap { get; set; }

        [JsonProperty("boostzoom")]
        public int BoostZoom { get; set; }

        [JsonProperty("compassview")]
        public string CompassView { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("image-format")]
        public string ImageFormat { get; set; }

        [JsonProperty("inclination")]
        public double Inclination { get; set; }

        [JsonProperty("lighting")]
        public string Lighting { get; set; }

        [JsonProperty("maptoworld")]
        public List<double> MapToWorld { get; set; }

        [JsonProperty("mapzoomin")]
        public int MapZoomIn { get; set; }

        [JsonProperty("mapzoomout")]
        public int MapZoomOut { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nightandday")]
        public bool NightAndDay { get; set; }

        [JsonProperty("perspective")]
        public string Perspective { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; }

        [JsonProperty("shader")]
        public string Shader { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("worldtomap")]
        public List<double> WorldToMap { get; set; }
    }
}