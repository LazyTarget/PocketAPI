using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PocketAPI
{
    public class Item
    {
        public Item()
        {
            Status = ItemStatus.New;
            Tags = new List<string>();
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

        [JsonIgnore]
        public IEnumerable<string> Tags
        {
            get
            {
                foreach (var prop in RawTags.Properties())
                {
                    yield return prop.Name;
                }
            }
            set
            {
                var tagEntity = new JObject();
                tagEntity["item_id"] = ItemID;

                var jObj = new JObject();
                foreach (var tag in value)
                {
                    tagEntity["tag"] = tag;
                    jObj.Add(tag, tagEntity);
                }
                RawTags = jObj;
            }
        }

        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore)]
        public JObject RawTags { get; set; } 


        public override string ToString()
        {
            var res = ResolvedTitle ?? GivenTitle;
            return res;
        }
    }

    public class Tag
    {
        [JsonProperty(PropertyName = "item_id")]
        public long ItemID { get; set; }
        
        [JsonProperty(PropertyName = "tag")]
        public string Name { get; set; }
    }
}
