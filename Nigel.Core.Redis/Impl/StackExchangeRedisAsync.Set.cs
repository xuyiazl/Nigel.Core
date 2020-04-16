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
    public abstract partial class StackExchangeRedis : ISetRedisCommandAsync
    {
        public async Task<bool> SetAddAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.SetAddAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<IList<T>> SetMembersAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetMembersAsync(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<bool> SetExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.SetContainsAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.SetRemoveAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<T> SetPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetPopAsync(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<long> SetLengthAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.SetLengthAsync(key);
            });
        }

        public async Task<T> SetRandomAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetRandomMemberAsync(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }
    }
}
