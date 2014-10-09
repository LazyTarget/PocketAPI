using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class TagsReplacePocketAction : PocketAction
    {
        public TagsReplacePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "tags_replace"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Set tags '{0}' for item #{1}", Tags, ItemID);
            return res;
        }
    }
}
