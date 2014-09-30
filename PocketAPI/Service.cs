using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PocketAPI
{
    public class Service
    {
        private const string AuthRequestURL = "https://getpocket.com/v3/oauth/request";
        private const string AuthGetAccessURL = "https://getpocket.com/v3/oauth/authorize";
        private const string AuthConfirmURL = "https://getpocket.com/auth/authorize?request_token={TOKEN}&redirect_uri={REDIRECT}";

        private string _requestCode;
        private string _accessCode;


        public string ConsumerKey { get; set; }

        public string RedirectUri { get; set; }

        public event EventHandler<ConfirmEventArgs> OnConfirmRequired;



        private static string SerializeJson(object obj)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }


        private static HttpWebResponse Post(string url, string json)
        {
            try
            {
                var request = WebRequest.CreateHttp(url);
                request.Accept = null;//"application/json";
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers.Add("X-Accept", "application/json");
                request.Method = "POST";

                var byteArray = Encoding.UTF8.GetBytes(json);

                var stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();


                var response = request.GetResponse();

                var httpResponse = (HttpWebResponse)response;
                return httpResponse;

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    stream = response.GetResponseStream();

                    var s = new StreamReader(stream, Encoding.UTF8);
                    var result = s.ReadToEnd();



                    dynamic d = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    var code = d.code;

                    return httpResponse;
                }
                else
                {
                    var error = httpResponse.GetResponseHeader("X-Error");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }


        private static dynamic GetResponseData(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            var s = new StreamReader(stream, Encoding.UTF8);
            var result = s.ReadToEnd();

            dynamic d = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            return d;
        }

        private static T GetResponseData<T>(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            var s = new StreamReader(stream, Encoding.UTF8);
            var result = s.ReadToEnd();

            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
            return d;
        }


        public void Authenticate(bool force = false)
        {
            if (_requestCode == null || force)
            {
                _requestCode = GetRequestToken();
            }

            if (_accessCode == null || force)
            {
                var confirmUrl = AuthConfirmURL.Replace("{TOKEN}", _requestCode)
                                               .Replace("{REDIRECT}", RedirectUri);

                if (OnConfirmRequired != null)
                    OnConfirmRequired(this, new ConfirmEventArgs(confirmUrl, AuthenticateCallback));        // client application shows webpage of Pocket's app authentication page
                else
                {
                    throw new InvalidOperationException("No 'OnConfirmRequired' event");
                    AuthenticateCallback();
                }
            }
        }

        private void AuthenticateCallback()
        {
            _accessCode = GetAccessToken();
        }


        public void Deauthenticate()
        {
            // soft logout (next request will trigger relogin)
            // Pocket servers dont know that user has logged off

            _requestCode = null;
            _accessCode = null;
        }



        private string GetRequestToken()
        {
            try
            {
                var requestData = new Dictionary<string, object>();
                requestData.Add("consumer_key", ConsumerKey);
                requestData.Add("redirect_uri", RedirectUri);
                var json = SerializeJson(requestData);

                var response = Post(AuthRequestURL, json);
                var responseData = GetResponseData(response);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var code = responseData.code;
                    if (code == null)
                        throw new Exception("Auth code not received");
                    return code;
                }
                else
                {
                    var error = response.GetResponseHeader("X-Error");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }


        private string GetAccessToken()
        {
            string username;
            return GetAccessToken(out username);
        }

        private string GetAccessToken(out string username)
        {
            try
            {
                var requestData = new Dictionary<string, object>();
                requestData.Add("consumer_key", ConsumerKey);
                requestData.Add("code", _requestCode);

                var json = SerializeJson(requestData);

                var url = AuthGetAccessURL;
                var response = Post(url, json);

                var responseData = GetResponseData(response);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = _accessCode = responseData.access_token;
                    username = responseData.username;
                    if (accessToken == null)
                        throw new Exception("AccessToken not received");

                    return accessToken;
                }
                else
                {
                    var error = response.GetResponseHeader("X-Error");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            username = null;
            return null;
        }


        public IEnumerable<Item> GetItems()
        {
            Authenticate();

            var url = "https://getpocket.com/v3/get";
            var requestData = new Dictionary<string, object>();
            requestData.Add("consumer_key", ConsumerKey);
            requestData.Add("access_token", _accessCode);

            requestData.Add("count", 10);
            requestData.Add("state", "all");


            var json = SerializeJson(requestData);

            var response = Post(url, json);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseData = GetResponseData(response);
                var data = (JObject) responseData;
                var list = (JObject) data.GetValue("list");
                
                foreach (JProperty i in list.Properties())
                {
                    var item = Item.FromJToken(i.Value);
                    yield return item;
                }
            }
            else
            {
                var error = response.GetResponseHeader("X-Error");
            }
        }


    }
}
