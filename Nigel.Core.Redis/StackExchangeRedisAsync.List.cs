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
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.ListLeftPushAsync(key, value.SafeString(), When.Exists, CommandFlags.None);
                else
                    return await db.ListLeftPushAsync(key, value.ToJson(), When.Exists, CommandFlags.None);
            });
        }

        public async Task<long> ListLeftPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].GetType() == typeof(string))
                        values[i] = value[i].SafeString();
                    else
                        values[i] = value[i].ToJson();
                }
                return await db.ListLeftPushAsync(key, values, CommandFlags.None);
            });
        }

        public async Task<long> ListLeftPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.ListLeftPushAsync(key, value.SafeString(), When.Always, CommandFlags.None);
                else
                    return await db.ListLeftPushAsync(key, value.ToJson(), When.Always, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushWhenExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.ListRightPushAsync(key, value.SafeString(), When.Exists, CommandFlags.None);
                else
                    return await db.ListRightPushAsync(key, value.ToJson(), When.Exists, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.ListRightPushAsync(key, value.SafeString(), When.Always, CommandFlags.None);
                else
                    return await db.ListRightPushAsync(key, value.ToJson(), When.Always, CommandFlags.None);
            });
        }

        public async Task<long> ListRightPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].GetType() == typeof(string))
                        values[i] = value[i].SafeString();
                    else
                        values[i] = value[i].ToJson();
                }
                return await db.ListRightPushAsync(key, values, CommandFlags.None);
            });
        }

        public async Task<T> ListLeftPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                string value = await db.ListLeftPopAsync(key);
                return value.ToObject<T>();
            });
        }

        public async Task<T> ListRightPopAsync<T>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                string value = await db.ListRightPopAsync(key);
                return value.ToObject<T>();
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
                if (value == null) return;
                if (value.GetType() == typeof(string))
                    await db.ListSetByIndexAsync(key, index, value.SafeString());
                else
                    await db.ListSetByIndexAsync(key, index, value.ToJson());
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
                string value = await db.ListGetByIndexAsync(key, index);
                return value.ToObject<T>();
            });
        }

        public async Task<IList<T>> ListRangeAsync<T>(string key, long start, long end, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var result = await db.ListRangeAsync(key, start, end);
                return result.ToStringArray().ToObjectNotNullOrEmpty<T>();
            });
        }

        public async Task<long> ListInsertBeforeAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
                 if (value == null) return 0;
                 if (value.GetType() == typeof(string))
                     return await db.ListInsertBeforeAsync(key, value.SafeString(), insertvalue);
                 else
                     return await db.ListInsertBeforeAsync(key, value.ToJson(), insertvalue);
             });
        }

        public async Task<long> ListInsertAfterAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
                 if (value == null) return 0;
                 if (value.GetType() == typeof(string))
                     return await db.ListInsertAfterAsync(key, value.SafeString(), insertvalue);
                 else
                     return await db.ListInsertAfterAsync(key, value.ToJson(), insertvalue);
             });
        }

        public async Task<long> ListRemoveAsync<T>(string key, T value, long removecount, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.ListRemoveAsync(key, value.SafeString(), removecount);
                else
                    return await db.ListRemoveAsync(key, value.ToJson(), removecount);
            });
        }
    }
}
