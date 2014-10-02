﻿namespace PocketAPI
{
    public interface IHttpHelper
    {
        /// <summary>
        /// Sends a http web request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IHttpHelperResponse Send(IHttpHelperRequest request);


        /// <summary>
        /// Sends a http web request
        /// </summary>
        /// <typeparam name="T">Result data type</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        IHttpHelperResponse<T> Send<T>(IHttpHelperRequest request);


    }
}
