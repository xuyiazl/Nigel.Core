using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    public static class HttpAuthorizationHeaderExtensions
    {
        public static HttpClient SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }
        public static HttpClient SetBearerToken(this HttpClient client, string token)
        {
            client.SetToken("Bearer", token);

            return client;
        }
        public static HttpRequestMessage SetToken(this HttpRequestMessage request, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);

            return request;
        }
        public static HttpRequestMessage SetBearerToken(this HttpRequestMessage request, string token)
        {
            request.SetToken("Bearer", token);

            return request;
        }
    }
}
