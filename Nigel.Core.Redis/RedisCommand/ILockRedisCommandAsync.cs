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
        Task<bool> LockExtendAsync(string key, string value, int seconds, string connectionName = null);

        Task<string> LockQueryAsync(string key, string connectionName = null);

        Task<bool> LockReleaseAsync(string key, string value, string connectionName = null);

        Task<bool> LockTakeAsync(string key, string value, int seconds, string connectionName = null);

    }
}
