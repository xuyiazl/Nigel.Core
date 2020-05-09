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
        public ILogger<HttpMessageService> _logger { get; set; }

        public HttpMessageService(ILogger<HttpMessageService> logger, IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
            this._logger = logger;
        }

        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(UrlArguments urlArguments, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Get, null, HttpMediaType.Json, headersCall, cancellationToken);

        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(UrlArguments urlArguments, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Get, null, mediaType, headersCall, cancellationToken);

        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="formData">formdata（application/x-www-form-urlencoded）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Post, formData, headersCall, cancellationToken);

        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Post, postData, HttpMediaType.Json, headersCall, cancellationToken);

        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Post, postData, mediaType, headersCall, cancellationToken);

        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="formData">formdata（application/x-www-form-urlencoded）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Put, formData, headersCall, cancellationToken);

        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Put, postData, HttpMediaType.Json, headersCall, cancellationToken);

        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Put, postData, mediaType, headersCall, cancellationToken);

        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="formData">formdata（application/x-www-form-urlencoded）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PatchAsync(UrlArguments urlArguments, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Patch, formData, headersCall, cancellationToken);

        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PatchAsync<TModel>(UrlArguments urlArguments, TModel postData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Patch, postData, HttpMediaType.Json, headersCall, cancellationToken);

        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PatchAsync<TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync<TModel>(urlArguments, HttpMethod.Patch, postData, mediaType, headersCall, cancellationToken);

        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(UrlArguments urlArguments, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Delete, null, HttpMediaType.Json, headersCall, cancellationToken);

        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(UrlArguments urlArguments, HttpMediaType mediaType, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, HttpMethod.Delete, null, mediaType, headersCall, cancellationToken);

        private async Task<HttpResponseMessage> HttpSendAsync(UrlArguments urlArguments, HttpMethod method, HttpFormData formData, Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(
                urlArguments,
                method,
                () => formData == null || formData.IsEmpty ? null : new StringContent(formData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"),
                 HttpMediaType.Json,
                 headersCall,
                cancellationToken);

        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="method">请求类型</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> HttpSendAsync<TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType mediaType = HttpMediaType.Json,
            Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
            => await HttpSendAsync(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJson(), Encoding.UTF8, "application/json"), mediaType, headersCall, cancellationToken);

        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="method">请求类型</param>
        /// <param name="contentCall">HttpContent请求内容</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="headersCall"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> HttpSendAsync(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType mediaType = HttpMediaType.Json,
            Action<HttpRequestHeaders> headersCall = null, CancellationToken cancellationToken = default)
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            string _mediaType = mediaType.Description();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

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

                if (headersCall != null)
                    headersCall.Invoke(requestMessage.Headers);

                requestMessage.Content = contentCall?.Invoke();
                //if (requestMessage.Content != null)
                //    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                RequestHeaders(client.DefaultRequestHeaders);

                if (headersCall != null)
                    headersCall.Invoke(client.DefaultRequestHeaders);

                HttpContent content = contentCall?.Invoke();
                //if (content != null)
                //    content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

                responseMessage = await SendAsync(client, requestUrl, method, content, cancellationToken);
            }

            return responseMessage;
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
