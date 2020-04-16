using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.Redis.RedisCommand
{
    /// <summary>
    /// Lock 命令
    /// </summary>
    public interface ILockRedisCommand
    {
        bool LockExtend<T>(string key, T value, int seconds, string connectionName = null);

        string LockQuery(string key, string connectionName = null);

        bool LockRelease<T>(string key, T value, string connectionName = null);

        bool LockTake<T>(string key, T value, int seconds, string connectionName = null);
    }
}
