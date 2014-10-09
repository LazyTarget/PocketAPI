using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class TagsRemovePocketAction : PocketAction
    {
        public TagsRemovePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "tags_remove"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Remove tags '{0}' from item #{1}", Tags, ItemID);
            return res;
        }
    }
}
