using Dynmap.Models;
using Dynmap.Models.Components;
using Newtonsoft.Json.Linq;

namespace Dynmap
{
    public class ComponentConverter : JsonCreationConverter<Component>
    {
        protected override Component Create(Type objectType, JObject jObject)
        {
            var type = jObject.Value<string>("type");
            return type switch
            {
                "chat" => new Chat(),
                "chatballoon" => new ChatBalloon(),
                "chatbox" => new ChatBox(),
                "coord" => new Coord(),
                "link" => new Link(),
                "markers" => new Markers(),
                "playermarkers" => new PlayerMarkers(),
                "timeofdayclock" => new TimeOfDayClock(),
                _ => new Component()
            };
        }
    }
}