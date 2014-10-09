using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class TagsAddPocketAction : PocketAction
    {
        public TagsAddPocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "tags_add"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Add tags '{0}' to item #{1}", Tags, ItemID);
            return res;
        }
    }
}
