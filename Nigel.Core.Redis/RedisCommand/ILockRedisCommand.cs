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
        bool LockExtend(string key, string value, int seconds, string connectionName = null);

        string LockQuery(string key, string connectionName = null);

        bool LockRelease(string key, string value, string connectionName = null);

        bool LockTake(string key, string value, int seconds, string connectionName = null);
    }
}
