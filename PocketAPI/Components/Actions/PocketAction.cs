using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public abstract class PocketAction
    {
        protected PocketAction()
        {
            Time = DateTime.UtcNow;
        }


        [JsonProperty(PropertyName = "action")]
        public abstract string Action { get; }


        [JsonProperty(PropertyName = "item_id")]
        public int ItemID { get; set; }


        [JsonProperty(PropertyName = "time"), JsonIgnore]       // todo: make it work!!
        public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("{0}: #{1}", Action, ItemID);
            return res;
        }
    }
}
