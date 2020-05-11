using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    public static class HttpAuthorizationHeaderExtensions
    {
        public static void SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }
        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.SetToken("Bearer", token);
        }
        public static void SetToken(this HttpRequestMessage request, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);
        }
        public static void SetBearerToken(this HttpRequestMessage request, string token)
        {
            request.SetToken("Bearer", token);
        }
    }
}
