using System.Collections.Generic;

namespace PocketAPI
{
    public interface IPocketAPI
    {
        void Authenticate();

        IEnumerable<Item> GetItems();


    }
}
