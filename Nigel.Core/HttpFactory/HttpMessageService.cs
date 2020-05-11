using Microsoft.Extensions.Logging;
using Nigel.Extensions;
using Nigel.Helpers;
using Nigel.Json;
using Nigel.Webs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    public class HttpMessageService : IHttpMessageService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }

        public HttpMessageService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        public HttpClient CreateClient(string clientName = "", HttpMediaType mediaType = HttpMediaType.Json)
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(clientName) ? "apiClient" : clientName);

            string _mediaType = mediaType.Description();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            return client;
        }

    }
}
