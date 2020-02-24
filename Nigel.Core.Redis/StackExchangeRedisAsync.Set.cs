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
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return await db.SetAddAsync(key, value.SafeString());
                else
                    return await db.SetAddAsync(key, value.ToJson());
            });
        }

        public async Task<string[]> SetMembersAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var Redisresult = await db.SetMembersAsync(key);
                return Redisresult.ToStringArray();
            });
        }

        public async Task<bool> SetExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return await db.SetContainsAsync(key, value.SafeString());
                else
                    return await db.SetContainsAsync(key, value.ToJson());
            });
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return await db.SetRemoveAsync(key, value.SafeString());
                else
                    return await db.SetRemoveAsync(key, value.ToJson());
            });
        }

        public async Task<T> SetPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                string value = await db.SetPopAsync(key);
                return value.ToObject<T>();
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
                string value = await db.SetRandomMemberAsync(key);

                return value.ToObject<T>();
            });
        }
    }
}
