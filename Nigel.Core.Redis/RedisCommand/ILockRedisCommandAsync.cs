using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.Redis.RedisCommand
{
    /// <summary>
    /// Lock 命令
    /// </summary>
    public interface ILockRedisCommandAsync
    {
        Task<bool> LockExtendAsync<T>(string key, T value, int seconds, string connectionName = null);

        Task<string> LockQueryAsync(string key, string connectionName = null);

        Task<bool> LockReleaseAsync<T>(string key, T value, string connectionName = null);

        Task<bool> LockTakeAsync<T>(string key, T value, int seconds, string connectionName = null);

    }
}
