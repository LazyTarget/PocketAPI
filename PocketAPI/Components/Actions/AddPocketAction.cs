using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class AddPocketAction : PocketAction
    {
        public AddPocketAction()
        {
            Time = DateTime.UtcNow;
        }


        public override string Action
        {
            get { return "add"; }
        }

        //[JsonProperty(PropertyName = "item_id")]
        //public int ItemID { get; set; }

        [JsonProperty(PropertyName = "ref_id")]
        public int RefID { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        //[JsonProperty(PropertyName = "time", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        //public DateTime Time { get; set; }


        public override string ToString()
        {
            var res = string.Format("Add item '{0}'", Url);
            return res;
        }
    }
}
