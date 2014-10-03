using System;
using Newtonsoft.Json.Linq;

namespace PocketAPI
{
    public class Item
    {
        public long ItemID { get; set; }

        public long ResolvedID { get; set; }

        public string GivenUrl { get; set; }

        public string ResolvedUrl { get; set; }

        public string GivenTitle { get; set; }

        public string ResolvedTitle { get; set; }

        public ItemStatus Status { get; set; }

        public bool Favorite { get; set; }

        public bool IsArticle { get; set; }


        public static Item FromJToken(JToken j)
        {
            var res = new Item();
            res.ItemID = (long)j.SelectToken("item_id");
            res.ResolvedID = (long)j.SelectToken("resolved_id");
            res.GivenUrl = (string)j.SelectToken("given_url");
            res.GivenTitle = (string)j.SelectToken("given_title");
            res.ResolvedUrl = (string)j.SelectToken("resolved_url");
            res.ResolvedTitle = (string)j.SelectToken("resolved_title");

            var tmp = j.SelectToken("favorite");
            if (tmp != null)
                res.Favorite = Convert.ToBoolean((int) tmp);

            tmp = j.SelectToken("is_article");
            if (tmp != null)
                res.IsArticle = Convert.ToBoolean((int) tmp);

            ItemStatus itemStatus;
            if (Enum.TryParse((string)j.SelectToken("status"), out itemStatus))
                res.Status = itemStatus;

            return res;
        }

    }

}
