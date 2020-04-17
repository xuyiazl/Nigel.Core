using Nigel.Webs;
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


        #region [ GET ]

        /// <summary>
        /// 异步GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new();

        /// <summary>
        /// 异步GET MessagePack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new();

        #endregion [ GET ]

        #region [ POST ]

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="formData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
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
        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();
        /// <summary>
        /// 异步POST MessagePack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PostMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();

        #endregion [ POST ]

        #region [ PUT ]
        /// <summary>
        /// 异步PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="formData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PutAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
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
        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();
        /// <summary>
        /// 异步PUT MessagePack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PutMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();

        #endregion [ PUT ]

        #region [ PATCH ]

        /// <summary>
        /// 异步PATCH
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="formData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PatchAsync<T>(UrlArguments urlArguments, HttpFormData formData, CancellationToken cancellationToken = default)
            where T : class, new();
        /// <summary>
        /// 异步PATCH
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();
        /// <summary>
        /// 异步PATCH MessagePack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PatchMsgPackAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new();

        #endregion [ PATCH ]

        #region [ DELETE ]
        /// <summary>
        /// 异步DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
             where T : class, new();
        /// <summary>
        /// 异步DELETE MessagePack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> DeleteMsgPackAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
             where T : class, new();

        #endregion [ DELETE ]
    }
}