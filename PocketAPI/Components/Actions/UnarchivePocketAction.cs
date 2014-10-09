using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class UnarchivePocketAction : PocketAction
    {
        public UnarchivePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "readd"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Unarchive item #{0}", ItemID);
            return res;
        }
    }
}
