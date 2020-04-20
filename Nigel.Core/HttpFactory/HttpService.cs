using MessagePack.Resolvers;
using Nigel.Helpers;
using Nigel.Json;
using Nigel.Webs;
using Nigel.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    public class HttpService : IHttpService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }
        public ILogger<HttpService> _logger { get; set; }

        public HttpService(ILogger<HttpService> logger, IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
            this._logger = logger;
        }

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), cancellationToken);

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, HttpMediaType httpData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, () => null, httpData, cancellationToken);

        public async Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Post, formData, cancellationToken);

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, HttpMediaType.Json, cancellationToken);

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType httpData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, httpData, cancellationToken);

        public async Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Put, formData, cancellationToken);

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, HttpMediaType.Json, cancellationToken);

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType httpData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, httpData, cancellationToken);

        public async Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Patch, formData, cancellationToken);

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, HttpMediaType.Json, cancellationToken);

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType httpData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, httpData, cancellationToken);

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), cancellationToken);

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, HttpMediaType httpData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, () => null, httpData, cancellationToken);

        private async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(
                urlArguments,
                method,
                () => formData == null || formData.IsEmpty ? null : new StringContent(formData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"),
                 HttpMediaType.Json,
                cancellationToken);

        public async Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType httpData = HttpMediaType.Json, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            switch (httpData)
            {
                case HttpMediaType.MessagePack:
                    {
                        return await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new ByteArrayContent(postData.ToMsgPackBytes()), httpData, cancellationToken);
                    }
                default:
                    {
                        return await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJson(), Encoding.UTF8, "application/json"), httpData, cancellationToken);
                    }
            }
        }

        public virtual async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType httpData = HttpMediaType.Json, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            string mediaType = httpData.Description();
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            HttpResponseMessage responseMessage = null;

            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };

                foreach (var accept in client.DefaultRequestHeaders.Accept)
                    requestMessage.Headers.Accept.Add(accept);

                RequestHeaders(requestMessage.Headers);

                requestMessage.Content = contentCall?.Invoke();
                if (requestMessage.Content != null)
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                RequestHeaders(client.DefaultRequestHeaders);

                HttpContent content = contentCall?.Invoke();
                if (content != null)
                    content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

                responseMessage = await SendAsync(client, requestUrl, method, content, cancellationToken);
            }

            switch (httpData)
            {
                case HttpMediaType.MessagePack:
                    {
                        var res = await responseMessage.Content.ReadAsByteArrayAsync();

                        if (_logger.IsEnabled(LogLevel.Information))
                            _logger.LogInformation($"{client.BaseAddress}{requestUrl} MediaType：{httpData.Description()}，Method：{method.Method}，HttpMessage Read Byte Data Length：{res.Length}");

                        return res.ToMsgPackObject<T>();
                    }
                default:
                    {
                        var res = await responseMessage.Content.ReadAsStringAsync();

                        if (_logger.IsEnabled(LogLevel.Information))
                            _logger.LogInformation($"{client.BaseAddress}{requestUrl} MediaType：{httpData.Description()}，Method：{method.Method}，HttpMessage Read Json Data：{res}");

                        return res.ToObject<T>();
                    }
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpClient client, string requestUrl, HttpMethod method, HttpContent content, CancellationToken cancellationToken)
        {
            switch (method.Method)
            {
                case "GET":
                    return await client.GetAsync(requestUrl, cancellationToken);
                case "POST":
                    return await client.PostAsync(requestUrl, content, cancellationToken);
                case "PUT":
                    return await client.PutAsync(requestUrl, content, cancellationToken);
                case "DELETE":
                    return await client.DeleteAsync(requestUrl, cancellationToken);
                case "PATCH":
                    return await client.PatchAsync(requestUrl, content, cancellationToken);
                default:
                    return await client.GetAsync(requestUrl, cancellationToken);
            }
        }

        /// <summary>
        /// 添加Headers消息头
        /// </summary>
        /// <param name="headers">header</param>
        public virtual void RequestHeaders(HttpRequestHeaders headers)
        {
            headers.Add("Client-IP", Web.IP);
        }
    }
}