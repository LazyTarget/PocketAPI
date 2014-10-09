using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class TagsClearPocketAction : PocketAction
    {
        public TagsClearPocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "tags_clear"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Clear tags for item #{0}", ItemID);
            return res;
        }
    }
}
