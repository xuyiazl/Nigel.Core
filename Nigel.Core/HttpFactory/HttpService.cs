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

        #region [ GET ]

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), cancellationToken);

        public async Task<T> GetMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, HttpMethod.Get, null, cancellationToken);

        #endregion [ GET ]

        #region [ POST ]

        public async Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Post, formData, cancellationToken);

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, cancellationToken);

        public async Task<T> PostMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, cancellationToken);

        #endregion [ POST ]

        #region [ PUT ]

        public async Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Put, formData, cancellationToken);

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, cancellationToken);

        public async Task<T> PutMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, cancellationToken);

        #endregion [ PUT ]

        #region [ PATCH ]


        public async Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Patch, formData, cancellationToken);

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, cancellationToken);

        public async Task<T> PatchMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, cancellationToken);

        #endregion [ PATCH ]

        #region [ DELETE ]

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), cancellationToken);

        public async Task<T> DeleteMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, HttpMethod.Delete, default, cancellationToken);

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

            /*

            //如果使用了 nginx 反向代理，需要在nginx里配置，并使用该方法获取IP

            if (_accessor.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
                ipAddress = _accessor.HttpContext.Request.Headers["X-Real-IP"].NullToEmpty();
            else
                ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.NullToEmpty();

            */

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
                {
                    var content = contentCall();

                    if (content != null)
                        requestMessage.Content = content;
                }

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                RequestHeaders(client.DefaultRequestHeaders);

                switch (method.Method)
                {
                    case "GET":
                        responseMessage = await client.GetAsync(requestUrl, cancellationToken);
                        break;
                    case "POST":
                        responseMessage = await client.PostAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                    case "PUT":
                        responseMessage = await client.PutAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                    case "DELETE":
                        responseMessage = await client.DeleteAsync(requestUrl, cancellationToken);
                        break;
                    case "PATCH":
                        responseMessage = await client.PatchAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                }
            }
            string res = await responseMessage.Content.ReadAsStringAsync();

            return res.ToObject<T>();
        }

        private async Task<T> HttpSendMsgPackAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendMsgPackAsync<T>(urlArguments, method, () => postData == null ? null : new ByteArrayContent(postData.ToMsgPackBytes()), cancellationToken);

        public virtual async Task<T> HttpSendMsgPackAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            /*

            //如果使用了 nginx 反向代理，需要在nginx里配置，并使用该方法获取IP

            if (_accessor.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
                ipAddress = _accessor.HttpContext.Request.Headers["X-Real-IP"].NullToEmpty();
            else
                ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.NullToEmpty();

            */

            HttpResponseMessage responseMessage = null;

            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-msgpack"));
                RequestHeaders(requestMessage.Headers);

                if (contentCall != null)
                {
                    var content = contentCall();

                    if (content != null)
                        requestMessage.Content = content;
                }

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);

            }
            else
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-msgpack"));

                RequestHeaders(client.DefaultRequestHeaders);

                switch (method.Method)
                {
                    case "GET":
                        responseMessage = await client.GetAsync(requestUrl, cancellationToken);
                        break;
                    case "POST":
                        responseMessage = await client.PostAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                    case "PUT":
                        responseMessage = await client.PutAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                    case "DELETE":
                        responseMessage = await client.DeleteAsync(requestUrl, cancellationToken);
                        break;
                    case "PATCH":
                        responseMessage = await client.PatchAsync(requestUrl, contentCall(), cancellationToken);
                        break;
                }
            }

            var res = await responseMessage.Content.ReadAsStreamAsync();

            return res.ToMsgPackObject<T>();
        }

        /// <summary>
        /// 添加Headers消息头
        /// </summary>
        /// <param name="headers">header</param>
        public virtual void RequestHeaders(HttpRequestHeaders headers)
        {
            headers.Add("ClientIP", Web.IP);
        }

        #endregion [ 内部方法 ]
    }
}