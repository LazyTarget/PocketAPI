using Newtonsoft.Json;

namespace PocketAPI
{
    public abstract class PocketAction
    {
        [JsonProperty(PropertyName = "action")]
        public abstract string Action { get; }


        [JsonProperty(PropertyName = "item_id")]
        public int ItemID { get; set; }


        public override string ToString()
        {
            var res = string.Format("{0}: #{1}", Action, ItemID);
            return res;
        }
    }
}
