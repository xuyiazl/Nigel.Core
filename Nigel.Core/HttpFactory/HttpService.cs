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
        IHttpClientFactory HttpClientFactory;
        public HttpService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }
        public async Task<T> GetAsync<T>(UrlArguments urlArguments)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), CancellationToken.None);

        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, new HttpFormData(), cancellationToken);

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

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), CancellationToken.None);

        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, new HttpFormData(), cancellationToken);

        private async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, HttpFormData formData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => formData == null || formData.IsEmpty ? null : new StringContent(formData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"), cancellationToken);

        private async Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, CancellationToken cancellationToken)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJsonString(), Encoding.UTF8, "application/json"), cancellationToken);

        public virtual async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, CancellationToken cancellationToken)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient("apiClient");

            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(urlArguments.Complete().Url)
            };

            requestMessage.Headers.Add("Accept-Encoding", "gzip");
            requestMessage.Headers.Add("ContentType", "application/json");

            var content = contentCall();

            if (content != null)
            {
                requestMessage.Content = content;
            }

            HttpResponseMessage responseMessage;

            if (cancellationToken == CancellationToken.None)
                responseMessage = await client.SendAsync(requestMessage);
            else
                responseMessage = await client.SendAsync(requestMessage, cancellationToken);

            string res = await responseMessage.Content.ReadAsStringAsync();

            return res.ToJsonObject<T>();
        }
    }
}
