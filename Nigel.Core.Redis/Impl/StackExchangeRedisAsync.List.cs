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
    public abstract partial class StackExchangeRedis : IListRedisCommandAsync
    {
        public async Task<long> ListLeftPushWhenExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.ListLeftPushAsync(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public async Task<long> ListLeftPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    values[i] = redisSerializer.Serializer(value[i]);
                }
                return await db.ListLeftPushAsync(key, values, CommandFlags.None);
            });
        }

        public async Task<long> ListLeftPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.ListLeftPushAsync(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushWhenExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.ListRightPushAsync(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.ListRightPushAsync(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    values[i] = redisSerializer.Serializer(value[i]);
                }
                return await db.ListRightPushAsync(key, values, CommandFlags.None);
            });
        }

        public async Task<T> ListLeftPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.ListLeftPopAsync(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<T> ListRightPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.ListRightPopAsync(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<long> ListLengthAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.ListLengthAsync(key);
            });
        }

        public async Task ListSetByIndexAsync<T>(string key, long index, T value, string connectionName = null)
        {
            await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                await db.ListSetByIndexAsync(key, index, redisSerializer.Serializer(value));
            });
        }

        public async Task ListTrimAsync(string key, long index, long end, string connectionName = null)
        {
            await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                await db.ListTrimAsync(key, index, end);
            });
        }

        public async Task<T> ListGetByIndexAsync<T>(string key, long index, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.ListGetByIndexAsync(key, index);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<IList<T>> ListRangeAsync<T>(string key, long start, long end, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var result = await db.ListRangeAsync(key, start, end);
                return redisSerializer.Deserialize<T>(result);
            });
        }

        public async Task<long> ListInsertBeforeAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
                 return await db.ListInsertBeforeAsync(key, redisSerializer.Serializer(value), insertvalue);
             });
        }

        public async Task<long> ListInsertAfterAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
                 return await db.ListInsertAfterAsync(key, redisSerializer.Serializer(value), insertvalue);
             });
        }

        public async Task<long> ListRemoveAsync<T>(string key, T value, long removecount, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.ListRemoveAsync(key, redisSerializer.Serializer(value), removecount);
            });
        }
    }
}
