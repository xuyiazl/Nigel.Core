using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Nigel.Extensions;
using Nigel.Json;
using System.Threading.Tasks;
using Nigel.Core.Redis.RedisCommand;

namespace Nigel.Core.Redis
{
    public abstract partial class StackExchangeRedis : ILockRedisCommandAsync
    {
        public async Task<bool> LockExtendAsync<T>(string key, T value, int seconds, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.LockExtendAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

        public async Task<string> LockQueryAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.LockQueryAsync(key);
            });
        }

        public async Task<bool> LockReleaseAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.LockReleaseAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<bool> LockTakeAsync<T>(string key, T value, int seconds, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.LockTakeAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

    }
}
