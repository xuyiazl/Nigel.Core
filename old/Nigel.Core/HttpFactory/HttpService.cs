using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task<T> GetAsync<T>(UrlArguments urlArguments)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), CancellationToken.None);

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), cancellationToken);

        #endregion

        #region [ POST ]

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, CancellationToken.None);

        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, cancellationToken);

        public async Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Post, formData, CancellationToken.None);

        public async Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Post, formData, cancellationToken);

        #endregion

        #region [ PUT ]

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, CancellationToken.None);

        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, cancellationToken);

        public async Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Put, formData, CancellationToken.None);

        public async Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Put, formData, cancellationToken);

        #endregion

        #region [ PATCH ]

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, CancellationToken.None);

        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, cancellationToken);

        public async Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Patch, formData, CancellationToken.None);

        public async Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Patch, formData, cancellationToken);

        #endregion

        #region [ DELETE ]

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), CancellationToken.None);

        public async Task<T> DeleteAsync<T, TModel>(UrlArguments urlArguments, TModel data)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Delete, data, CancellationToken.None);

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), cancellationToken);

        public async Task<T> DeleteAsync<T, TModel>(UrlArguments urlArguments, TModel data, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Delete, data, cancellationToken);

        #endregion

        #region [ 内部方法 ]


        private async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, HttpFormData formData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => formData == null || formData.IsEmpty ? null : new StringContent(formData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"), cancellationToken);

        private async Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJsonString(), Encoding.UTF8, "application/json"), cancellationToken);

        public virtual async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, CancellationToken cancellationToken)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            /*

            string ipAddress = (headers.ContainsKey("HTTP_X_FORWARDED_FOR")) ? headers["HTTP_X_FORWARDED_FOR"].NullToEmpty() : headers["REMOTE_ADDR"].NullToEmpty();

            
            //如果使用了 nginx 反向代理，需要在nginx里配置，并使用该方法获取IP

            if (_accessor.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
                ipAddress = _accessor.HttpContext.Request.Headers["X-Real-IP"].NullToEmpty();
            else
                ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.NullToEmpty();

            */


            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };

                requestMessage.Headers.Add("Accept-Encoding", "gzip,deflate");
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.Headers.Add("ContentType", "application/json");

                var content = contentCall();

                if (content != null)
                    requestMessage.Content = content;

                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage, cancellationToken);

                string res = await responseMessage.Content.ReadAsStringAsync();

                return res.ToJsonObject<T>();
            }
            else
            {
                HttpResponseMessage responseMessage = null;

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

                string res = await responseMessage.Content.ReadAsStringAsync();

                return res.ToJsonObject<T>();

            }
        }

        #endregion
    }
}
