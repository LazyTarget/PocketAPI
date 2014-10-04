﻿using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PocketAPI
{
    public class JsonHttpHelper : HttpHelperBase
    {
        private JsonSerializerSettings Settings { get; set; }

        public JsonHttpHelper()
        {
            Settings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }


        protected override HttpWebRequest BuildRequest(IHttpHelperRequest request)
        {
            var httpWebRequest = base.BuildRequest(request);
            httpWebRequest.Accept = null;
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Headers.Add("X-Accept", "application/json");
            return httpWebRequest;
        }


        protected override void SetRequestPayload(IHttpHelperRequest request, Stream requestStream)
        {
            if (request.Data == null)
                return;

            var json = JsonConvert.SerializeObject(request.Data, Settings);
            var byteArray = Encoding.GetBytes(json);
            requestStream.Write(byteArray, 0, byteArray.Length);
        }


        protected override object ReadResponseData(IHttpHelperRequest request, Stream responseStream)
        {
            var responseData = base.ReadResponseData(request, responseStream);
            return responseData;
        }


        protected override T Deserialize<T>(object data)
        {
            T result;
            if (data is string)
            {
                var json = (string)data;
                result = JsonConvert.DeserializeObject<T>(json, Settings);
            }
            else
            {
                var json = JsonConvert.SerializeObject(data, Settings);
                result = JsonConvert.DeserializeObject<T>(json, Settings);
            }
            return result;
        }


    }

}
