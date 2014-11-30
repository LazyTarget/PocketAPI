namespace PocketAPI
{
    public class ItemTag
    {
        private string _tag;

        private ItemTag() { }
        private ItemTag(string tag)
        {
            _tag = tag;
        }


        public static implicit operator ItemTag(string s)
        {
            var tag = new ItemTag();
            tag._tag = s;
            return tag;
        }

        public static implicit operator string(ItemTag tag)
        {
            tag = tag ?? new ItemTag();
            return tag._tag;
        }

        public static readonly ItemTag Untagged = new ItemTag("_untagged_");

    }
}