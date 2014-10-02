using System;
using System.IO;
using System.Net;

namespace PocketAPI
{
    public class JsonHttpHelper : HttpHelperBase
    {
        protected override HttpWebRequest BuildRequest(IHttpHelperRequest request)
        {
            var httpWebRequest = base.BuildRequest(request);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Headers.Add("X-Accept", "application/json");
            return httpWebRequest;
        }


        protected override void SetRequestPayload(IHttpHelperRequest request, Stream requestStream)
        {
            if (request.Data == null)
                return;

            using (var sw = new StreamWriter(requestStream, Encoding))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(request.Data);
                sw.WriteLine(json);

                sw.Close();
            }
        }


        protected override object ReadResponseData(IHttpHelperRequest request, Stream responseStream)
        {
            var responseData = base.ReadResponseData(request, responseStream);
            return responseData;
        }


        protected override T Serialize<T>(object data)
        {
            T result;
            if (data is string)
            {
                var json = (string)data;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            return result;
        }


    }

}
