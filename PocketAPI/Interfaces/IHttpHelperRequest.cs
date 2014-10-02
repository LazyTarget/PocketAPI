using System.Net;

namespace PocketAPI
{
    public interface IHttpHelperRequest
    {
        string Url { get; set; }

        string Method { get; set; }

        object Data { get; set; }

        WebHeaderCollection Headers { get; set; }

    }
}