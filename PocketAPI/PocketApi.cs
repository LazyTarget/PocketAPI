using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PocketAPI
{
    public class PocketApi : IPocketAPI
    {
        private static class URL
        {
            public const string AuthRequestURL = "https://getpocket.com/v3/oauth/request.php";
            public const string AuthGetAccessURL = "https://getpocket.com/v3/oauth/authorize";
            public const string AuthConfirmURL = "https://getpocket.com/auth/authorize?request_token={TOKEN}&redirect_uri={REDIRECT}";

            public const string GetItems = "https://getpocket.com/v3/get";
        }


        private string _requestCode;
        private string _accessCode;


        public PocketApi()
        {
            HttpHelper = new JsonHttpHelper();
        }


        public IHttpHelper HttpHelper { get; set; }

        public string ConsumerKey { get; set; }

        public string RedirectUri { get; set; }

        public event EventHandler<ConfirmEventArgs> OnConfirmRequired;

        

        public void Authenticate()
        {
            Authenticate(false);
        }

        public void Authenticate(bool force)
        {
            if (_requestCode == null || force)
            {
                _requestCode = GetRequestToken();
            }

            if (_accessCode == null || force)
            {
                var confirmUrl = URL.AuthConfirmURL.Replace("{TOKEN}", _requestCode)
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

                var request = new HttpHelperRequest
                {
                    Data = requestData,
                    Url = URL.AuthRequestURL,
                    Method = HttpMethod.POST
                };
                var response = HttpHelper.Send<dynamic>(request);
                if (response.Response.StatusCode == HttpStatusCode.OK)
                {
                    var code = response.Result.code;
                    if (code == null)
                        throw new Exception("Auth code not received");
                    return code;
                }
                else
                    throw PocketException.Create(response.Response);
            }
            catch (WebException ex)
            {
                throw PocketException.Create(ex);
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

                var request = new HttpHelperRequest
                {
                    Data = requestData,
                    Url = URL.AuthGetAccessURL,
                    Method = HttpMethod.POST
                };
                var response = HttpHelper.Send<dynamic>(request);
                if (response.Response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = response.Result.access_token;
                    username = response.Result.username;
                    if (accessToken == null)
                        throw new Exception("AccessToken not received");

                    _accessCode = accessToken;
                    return accessToken;
                }
                else
                    throw PocketException.Create(response.Response);
            }
            catch (WebException ex)
            {
                throw PocketException.Create(ex);
            }
            username = null;
            return null;
        }


        private IEnumerable<Item> _GetItems()
        {
            Authenticate();

            var requestData = new Dictionary<string, object>();
            requestData.Add("consumer_key", ConsumerKey);
            requestData.Add("access_token", _accessCode);

            requestData.Add("count", 10);
            requestData.Add("state", "all");


            var request = new HttpHelperRequest
            {
                Data = requestData,
                Url = URL.GetItems,
                Method = HttpMethod.POST
            };
            var response = HttpHelper.Send<JObject>(request);

            if (response.Response.StatusCode == HttpStatusCode.OK)
            {
                var list = (JObject)response.Result.GetValue("list");

                foreach (JProperty i in list.Properties())
                {
                    var item = Item.FromJToken(i.Value);
                    yield return item;
                }
            }
            else
                throw PocketException.Create(response.Response);
        }



        public IEnumerable<Item> GetItems()
        {
            var enumerable = _GetItems();
            var enumerator = enumerable.GetEnumerator();
            enumerable = TryCatchEnumerable(enumerator);
            return enumerable;
        }



        private static IEnumerable<T> TryCatchEnumerable<T>(IEnumerator<T> enumerator)
        {
            while (true)
            {
                T item;
                try
                {
                    if (!enumerator.MoveNext())
                        break;
                    item = enumerator.Current;
                }
                catch (WebException ex)
                {
                    throw PocketException.Create(ex);
                }
                yield return item;
            }
        }


    }
}
