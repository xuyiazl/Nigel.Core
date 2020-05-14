using Microsoft.Extensions.Logging;
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
    public interface IHttpMessageService
    {
        IHttpClientFactory HttpClientFactory { get; set; }
        HttpClient CreateClient(string clientName = "");
    }
}
