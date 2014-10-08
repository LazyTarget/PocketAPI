using Newtonsoft.Json.Linq;

namespace PocketAPI
{
    public static class JsonSerializerExtensions
    {
        public static T ToObjectOrDefault<T>(this JToken token)
        {
            if (token == null)
                return default(T);
            var res = token.ToObject<T>();
            return res;
        }
    }
}
