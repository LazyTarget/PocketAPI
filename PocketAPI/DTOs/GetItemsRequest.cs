namespace PocketAPI
{
    public class GetItemsRequest
    {
        public GetItemsRequest()
        {
            Page = 1;
            PageSize = 10;
            Favorite = false;
            Sort = "newest";
            State = ItemState.Unread;
        }


        public string SearchQuery { get; set; }


        public ItemTag Tag { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool Favorite { get; set; }
        
        public string Sort { get; set; }

        public ItemState State { get; set; }


    }

    public enum ItemState
    {
        All,
        Unread,
        Archived,
    }

}
