using System.Net;

namespace PocketAPI
{
    public interface IHttpHelperResponse
    {
        HttpWebRequest Request { get; }

        HttpWebResponse Response { get; }

    }


    public interface IHttpHelperResponse<out T> : IHttpHelperResponse
    {
        T Result { get; }

    }
}
