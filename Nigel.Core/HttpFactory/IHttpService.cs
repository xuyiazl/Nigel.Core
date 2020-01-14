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

        /// <summary>
        /// 异步DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(UrlArguments urlArguments)
            where T : class, new();

        /// <summary>
        /// 异步DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<T> DeleteAsync<T, TModel>(UrlArguments urlArguments, TModel data)
            where T : class, new();

        /// <summary>
        /// 异步DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> DeleteAsync<T, TModel>(UrlArguments urlArguments, TModel data, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(UrlArguments urlArguments)
            where T : class, new();

        /// <summary>
        /// 异步GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
            where T : class, new();

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="urlParamter"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter)
            where T : class, new();

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="urlParamter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
            where T : class, new();

        /// <summary>
        /// 异步PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
            where T : class, new();

        /// <summary>
        /// 异步PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="urlParamter"></param>
        /// <returns></returns>
        Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter)
            where T : class, new();

        /// <summary>
        /// 异步PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="urlParamter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter, CancellationToken cancellationToken)
            where T : class, new();

        ///// <summary>
        ///// 异步PATCH
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TModel"></typeparam>
        ///// <param name="urlArguments"></param>
        ///// <param name="postData"></param>
        ///// <returns></returns>
        //Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData)
        //    where T : class, new();
        ///// <summary>
        ///// 异步PATCH
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TModel"></typeparam>
        ///// <param name="urlArguments"></param>
        ///// <param name="postData"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken)
        //    where T : class, new();
        ///// <summary>
        ///// 异步PATCH
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="urlArguments"></param>
        ///// <param name="urlParamter"></param>
        ///// <returns></returns>
        //Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter)
        //    where T : class, new();
        ///// <summary>
        ///// 异步PATCH
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="urlArguments"></param>
        ///// <param name="urlParamter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData urlParamter, CancellationToken cancellationToken)
        //    where T : class, new();
    }
}