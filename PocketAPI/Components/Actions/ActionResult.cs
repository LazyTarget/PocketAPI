namespace PocketAPI
{
    public class ActionResult
    {
        public PocketAction Action { get; internal set; }

        public object Result { get; internal set; }

        public bool Success { get; internal set; }


        public override string ToString()
        {
            var res = string.Format("{0} => {1}", Action, Success);
            if (Result is Item)
            {
                var item = (Item)Result;
                if (!string.IsNullOrWhiteSpace(item.ResolvedTitle))
                    res = string.Format("{0} ({2}) => {1}", Action, Success, item.ResolvedTitle);
            }
            return res;
        }
    }
}
