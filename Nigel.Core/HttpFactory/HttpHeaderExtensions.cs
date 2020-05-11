using Nigel.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    public static class HttpHeaderExtensions
    {
        public static HttpClient SetHeader(this HttpClient client, Action<HttpRequestHeaders> action)
        {
            action.Invoke(client.DefaultRequestHeaders);

            return client;
        }

        public static HttpClient SetHeaderToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }

        public static HttpClient SetHeaderBearerToken(this HttpClient client, string token)
        {
            client.SetHeaderToken("Bearer", token);

            return client;
        }

        public static HttpClient SetHeaderClientIP(this HttpClient client, string clientIP = "")
        {
            client.DefaultRequestHeaders.Add("Client-IP", string.IsNullOrEmpty(clientIP) ? Web.IP : clientIP);

            return client;
        }

        public static HttpRequestMessage SetHeader(this HttpRequestMessage request, Action<HttpRequestHeaders> action)
        {
            action.Invoke(request.Headers);

            return request;
        }

        public static HttpRequestMessage SetHeaderToken(this HttpRequestMessage request, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);

            return request;
        }

        public static HttpRequestMessage SetHeaderBearerToken(this HttpRequestMessage request, string token)
        {
            request.SetHeaderToken("Bearer", token);

            return request;
        }

        public static HttpRequestMessage SetHeaderClientIP(this HttpRequestMessage request, string clientIP = "")
        {
            request.Headers.Add("Client-IP", string.IsNullOrEmpty(clientIP) ? Web.IP : clientIP);

            return request;
        }

    }
}
