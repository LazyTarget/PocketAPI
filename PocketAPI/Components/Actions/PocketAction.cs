using Newtonsoft.Json;

namespace PocketAPI
{
    public abstract class PocketAction
    {
        [JsonProperty(PropertyName = "action")]
        public abstract string Action { get; }

    }
}
