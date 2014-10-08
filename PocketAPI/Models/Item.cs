using System;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class Item
    {
        public Item()
        {
            Status = ItemStatus.New;
        }

        [JsonProperty(PropertyName = "item_id")]
        public long ItemID { get; set; }

        [JsonProperty(PropertyName = "resolved_id")]
        public long ResolvedID { get; set; }

        [JsonProperty(PropertyName = "given_url")]
        public string GivenUrl { get; set; }

        [JsonProperty(PropertyName = "resolved_url")]
        public string ResolvedUrl { get; set; }

        [JsonProperty(PropertyName = "given_title")]
        public string GivenTitle { get; set; }

        [JsonProperty(PropertyName = "resolved_title")]
        public string ResolvedTitle { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public ItemStatus Status { get; set; }

        [JsonProperty(PropertyName = "favorite", NullValueHandling = NullValueHandling.Ignore)]
        public int Favorite { get; set; }

        [JsonProperty(PropertyName = "is_article", NullValueHandling = NullValueHandling.Ignore)]
        public int IsArticle { get; set; }
    }
}
