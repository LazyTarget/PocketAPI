namespace PocketAPI
{
    public class HttpMethod
    {
        private readonly string _method = "GET";

        private HttpMethod(string method)
        {
            _method = method;
        }


        public static implicit operator string(HttpMethod method)
        {
            return method._method;
        }


        public readonly static HttpMethod GET = new HttpMethod("GET");
        public readonly static HttpMethod POST = new HttpMethod("POST");
        public readonly static HttpMethod PUT = new HttpMethod("PUT");
        public readonly static HttpMethod DELETE = new HttpMethod("DELETE");
    }
}
