using System.Net;

namespace PocketAPI
{
    public static class HttpRequestExtensions
    {
        public static T WithUrl<T>(this T request, string url)
            where T : IHttpHelperRequest
        {
            request.Url = url;
            return request;
        }

        public static T WithMethod<T>(this T request, string method)
            where T : IHttpHelperRequest
        {
            request.Method = method;
            return request;
        }

        public static T WithData<T>(this T request, object data)
            where T : IHttpHelperRequest
        {
            request.Data = data;
            return request;
        }

        public static T WithHeaders<T>(this T request, WebHeaderCollection headers)
            where T : IHttpHelperRequest
        {
            request.Headers = headers;
            return request;
        }

    }
}
