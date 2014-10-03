using System;
using System.Net;
using System.Runtime.Serialization;

namespace PocketAPI
{
    public class PocketException : Exception
    {
        internal PocketException()
        {
        }

        internal PocketException(string message) : base(message)
        {
        }

        internal PocketException(string message, Exception innerException) : base(message, innerException)
        {
        }




        public int ErrorCode { get; private set; }





        internal static PocketException Create(WebException ex)
        {
            var httpWebResponse = (HttpWebResponse) ex.Response;
            var error = httpWebResponse.GetResponseHeader("X-Error");
            if (string.IsNullOrWhiteSpace(error))
                error = ex.Message;

            var pocketException = new PocketException(error, ex);
            pocketException.ErrorCode = Convert.ToInt32(httpWebResponse.GetResponseHeader("X-Error-Code"));
            return pocketException;
        }


        internal static PocketException Create(WebResponse webResponse)
        {
            var httpWebResponse = (HttpWebResponse) webResponse;
            var error = httpWebResponse.GetResponseHeader("X-Error");

            var pocketException = new PocketException(error);
            pocketException.ErrorCode = Convert.ToInt32(httpWebResponse.GetResponseHeader("X-Error-Code"));
            return pocketException;

        }

    }
}
