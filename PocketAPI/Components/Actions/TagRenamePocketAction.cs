using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class TagRenamePocketAction : PocketAction
    {
        public TagRenamePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "tag_rename"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "old_tag")]
        public string OldTag { get; set; }

        [JsonProperty(PropertyName = "new_tag")]
        public string NewTag { get; set; }

        [JsonProperty(PropertyName = "time"), JsonIgnore]
        public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Renaming tag '{0}' to '{1}' (#{2})", OldTag, NewTag, ItemID);
            return res;
        }
    }
}
