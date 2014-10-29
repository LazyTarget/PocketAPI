using System.IO;
using System.Net;
using System.Text;

namespace PocketAPI
{
    public abstract class HttpHelperBase : IHttpHelper
    {
        protected HttpHelperBase()
        {
            Timeout = 100000;
            Encoding = Encoding.UTF8;
        }


        /// <summary>
        /// Timeout in milliseconds, default is 100 secs
        /// </summary>
        public int Timeout { get; set; }


        public Encoding Encoding { get; set; }



        private IHttpHelperResponse Process(IHttpHelperRequest request)
        {
            var httpWebRequest = BuildRequest(request);
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var response = new HttpHelperResponse(httpWebRequest, httpWebResponse);
            return response;
        }

        private IHttpHelperResponse<T> Process<T>(IHttpHelperRequest request)
        {
            var httpWebRequest = BuildRequest(request);
            var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            var responseData = GetResponseData(request, httpWebResponse);
            var result = Deserialize<T>(responseData);
            var response = new HttpHelperResponse<T>(result, httpWebRequest, httpWebResponse);
            return response;
        }


        protected virtual HttpWebRequest BuildRequest(IHttpHelperRequest request)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            httpWebRequest.Method = request.Method ?? "GET";
            httpWebRequest.Timeout = Timeout;
            if (request.Headers != null && request.Headers.Count > 0)
                httpWebRequest.Headers.Add(request.Headers);


            // Set payload
            if (!httpWebRequest.Method.Equals(HttpMethod.GET, System.StringComparison.InvariantCultureIgnoreCase))
            {
                using (var stream = httpWebRequest.GetRequestStream())
                {
                    SetRequestPayload(request, stream);
                    stream.Close();
                }
            }

            return httpWebRequest;
        }


        protected virtual void SetRequestPayload(IHttpHelperRequest request, Stream requestStream)
        {
            if (request.Data == null)
                return;

            var dataString = request.Data.ToString();
            var byteArray = Encoding.GetBytes(dataString);
            requestStream.Write(byteArray, 0, byteArray.Length);

            //using (var sw = new StreamWriter(requestStream, Encoding))
            //{
            //    sw.WriteLine(request.Data);

            //    sw.Close();
            //}
        }



        private object GetResponseData(IHttpHelperRequest request, HttpWebResponse httpWebResponse)
        {
            object result;
            using (var stream = httpWebResponse.GetResponseStream())
            {
                result = ReadResponseData(request, stream);

                stream.Close();
            }
            return result;
        }


        protected virtual object ReadResponseData(IHttpHelperRequest request, Stream responseStream)
        {
            string resultString;
            using (var sr = new StreamReader(responseStream, Encoding))
            {
                resultString = sr.ReadToEnd();

                sr.Close();
            }
            return resultString;
        }



        protected virtual T Deserialize<T>(object data)
        {
            var result = (T) data;
            return result;
        }
        


        public IHttpHelperResponse Send(IHttpHelperRequest request)
        {
            var response = Process(request);
            return response;
        }

        public IHttpHelperResponse<T> Send<T>(IHttpHelperRequest request)
        {
            var response = Process<T>(request);
            return response;
        }

    }
}