using Nigel.Webs;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    public interface IHttpService
    {
        IHttpClientFactory HttpClientFactory { get; set; }

        Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> GetAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> DeleteAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
          where T : class, new();

        Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
          where T : class, new();
    }
}