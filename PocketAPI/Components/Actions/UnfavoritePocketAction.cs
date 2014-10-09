using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class UnfavoritePocketAction : PocketAction
    {
        public UnfavoritePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "unfavorite"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Unfavorite item #{0}", ItemID);
            return res;
        }
    }
}
