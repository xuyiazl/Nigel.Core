using MessagePack.Resolvers;
using Nigel.Helpers;
using Nigel.Json;
using Nigel.Webs;
using System;
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
    public class HttpService : IHttpService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }

        public HttpService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        #region [ JSON ]

        #region [ GET ]

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), cancellationToken);

        #endregion [ GET ]

        #region [ POST ]

        public async Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Post, formData, cancellationToken);

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, cancellationToken);

        #endregion [ POST ]

        #region [ PUT ]

        public async Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Put, formData, cancellationToken);

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, cancellationToken);


        #endregion [ PUT ]

        #region [ PATCH ]


        public async Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Patch, formData, cancellationToken);

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, cancellationToken);


        #endregion [ PATCH ]

        #region [ DELETE ]

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), cancellationToken);


        #endregion [ DELETE ]

        #region [ 内部方法 ]

        private async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => formData == null || formData.IsEmpty ? null : new StringContent(formData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"), cancellationToken);

        private async Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJson(), Encoding.UTF8, "application/json"), cancellationToken);

        public virtual async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            HttpResponseMessage responseMessage = null;

            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };

                RequestHeaders(requestMessage.Headers);

                if (contentCall != null)
                    requestMessage.Content = contentCall();

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                RequestHeaders(client.DefaultRequestHeaders);

                HttpContent content = default;
                if (contentCall != null)
                    content = contentCall();

                responseMessage = await SendAsync(client, requestUrl, method, content, cancellationToken);
            }
            string res = await responseMessage.Content.ReadAsStringAsync();

            return res.ToObject<T>();
        }

        #endregion [ 内部方法 ]

        #endregion

        #region [ MessagePack ]

        #region [ GET ]

        public async Task<T> GetMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, HttpMethod.Get, default, cancellationToken);

        #endregion [ GET ]

        #region [ POST ]

        public async Task<T> PostMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, cancellationToken);

        #endregion [ POST ]

        #region [ PUT ]

        public async Task<T> PutMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, cancellationToken);

        #endregion [ PUT ]

        #region [ PATCH ]

        public async Task<T> PatchMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, cancellationToken);

        #endregion [ PATCH ]

        #region [ DELETE ]

        public async Task<T> DeleteMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, HttpMethod.Delete, default, cancellationToken);

        #endregion [ DELETE ]

        #region [ 内部方法 ]

        private async Task<T> HttpSendMsgPackAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, method, () => postData == null ? null : new ByteArrayContent(postData.ToMsgPackBytes()), cancellationToken);

        public virtual async Task<T> HttpSendMsgPackAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            string contentType = "application/x-msgpack";

            HttpResponseMessage responseMessage = null;

            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };

                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                RequestHeaders(requestMessage.Headers);

                if (contentCall != null)
                {
                    requestMessage.Content = contentCall();
                    if (requestMessage.Content != null)
                        requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                RequestHeaders(client.DefaultRequestHeaders);

                HttpContent content = default;
                if (contentCall != null)
                {
                    content = contentCall();
                    if (content != null)
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                responseMessage = await SendAsync(client, requestUrl, method, content, cancellationToken);

            }

            var res = await responseMessage.Content.ReadAsStreamAsync();

            return res.ToMsgPackObject<T>();
        }

        #endregion

        #endregion

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
            headers.Add("ClientIP", Web.IP);
        }
    }
}