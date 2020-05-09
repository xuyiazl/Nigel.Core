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
        ILogger<HttpMessageService> _logger { get; set; }
        IHttpClientFactory HttpClientFactory { get; set; }

        Task<HttpResponseMessage> DeleteAsync(UrlArguments urlArguments, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteAsync(UrlArguments urlArguments, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> GetAsync(UrlArguments urlArguments, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> GetAsync(UrlArguments urlArguments, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> HttpSendAsync(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType mediaType = HttpMediaType.Json, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> HttpSendAsync<TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType mediaType = HttpMediaType.Json, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PatchAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PatchAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PatchAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PostAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PostAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PostAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PutAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PutAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PutAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default);
        void RequestHeaders(HttpRequestHeaders headers);
    }
}
