using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.Redis.RedisCommand
{
    /// <summary>
    /// String 命令
    /// </summary>
    public interface IStringRedisCommandAsync
    {
        /// <summary>
        /// 写入string命令key-value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">失效时间，默认不失效（当业务场景中需要做失效时间时使用）</param>
        /// <param name="connectionName">连接名称</param>
        Task StringSetAsync<T>(string key, T value, int seconds = 0, string connectionName = null);
        /// <summary>
        /// 获取string的value值，如果不存在则写入
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <param name="connectionRead"></param>
        /// <param name="connectionWrite"></param>
        /// <param name="fetcher"></param>
        /// <returns></returns>
        Task<TResult> StringGetOrInsertAsync<TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<TResult> fetcher);
        /// <summary>
        /// 获取string的value值，如果不存在则写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <param name="connectionRead"></param>
        /// <param name="connectionWrite"></param>
        /// <param name="fetcher"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<TResult> StringGetOrInsertAsync<T, TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t);
        /// <summary>
        /// 获得string的value值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        Task<TResult> StringGetAsync<TResult>(string key, string connectionName = null);
        /// <summary>
        /// 批量获得string类型的值
        /// </summary>
        /// <param name="keys">key数组</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        Task<List<TResult>> StringGetAsync<TResult>(string[] keys, string connectionName = null);
        /// <summary>
        /// 原子性自增列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long> StringIncrementAsync(string key, long value = 1, string connectionName = null);

    }
}
