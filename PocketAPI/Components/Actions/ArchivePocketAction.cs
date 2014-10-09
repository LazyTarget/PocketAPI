using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class ArchivePocketAction : PocketAction
    {
        public ArchivePocketAction()
        {
            Time = DateTime.UtcNow;
        }

        public override string Action
        {
            get { return "archive"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        //[JsonProperty(PropertyName = "time"), JsonIgnore]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Archive item #{0}", ItemID);
            return res;
        }
    }
}
