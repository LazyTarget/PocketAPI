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
            get { return "archive"; }
        }

        [JsonProperty(PropertyName = "item_id")]
        public int ItemID { get; set; }

        [JsonProperty(PropertyName = "ref_id")]
        public int RefID { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

    }
}
