using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class FavoritePocketAction : PocketAction
    {
        public FavoritePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "favorite"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "time"), JsonIgnore]
        public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Favorite item #{0}", ItemID);
            return res;
        }
    }
}
