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
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return await db.ListLeftPushAsync(key, value.SafeString(), When.Exists, CommandFlags.None);
                    else
                        return await db.ListLeftPushAsync(key, value.ToJson(), When.Exists, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public async Task<long> ListLeftPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value != null)
                    {
                        RedisValue[] values = new RedisValue[value.Count];
                        for (int i = 0; i < value.Count; i++)
                        {
                            values[i] = value[i].ToJson();
                        }
                        return await db.ListLeftPushAsync(key, values, CommandFlags.None);
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public async Task<long> ListLeftPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return await db.ListLeftPushAsync(key, value.SafeString(), When.Always, CommandFlags.None);
                    else
                        return await db.ListLeftPushAsync(key, value.ToJson(), When.Always, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public async Task<long> ListRightPushWhenExistsAsync<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return await db.ListRightPushAsync(key, value.SafeString(), When.Exists, CommandFlags.None);
                    else
                        return await db.ListRightPushAsync(key, value.ToJson(), When.Exists, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public async Task<long> ListRightPushWhenNoExistsAsync<T>(string key, T value, string connectionName = null)
        {
            long result = 0;
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        result = await db.ListRightPushAsync(key, value.SafeString(), When.Always, CommandFlags.None);
                    else
                        result = await db.ListRightPushAsync(key, value.ToJson(), When.Always, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return result;
        }

        public async Task<long> ListRightPushAsync<T>(string key, List<T> value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value != null)
                    {
                        RedisValue[] values = new RedisValue[value.Count];
                        for (int i = 0; i < value.Count; i++)
                        {
                            values[i] = value[i].ToJson();
                        }
                        return await db.ListRightPushAsync(key, values, CommandFlags.None);
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }
        
        public async Task<T> ListLeftPopAsync<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = await db.ListLeftPopAsync(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public async Task<T> ListRightPopAsync<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = await db.ListRightPopAsync(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public async Task<long> ListLengthAsync(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    return await db.ListLengthAsync(key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return 0;
        }

        public async Task ListSetByIndexAsync<T>(string key, long index, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        await db.ListSetByIndexAsync(key, index, value.SafeString());
                    else
                        await db.ListSetByIndexAsync(key, index, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public async Task ListTrimAsync(string key, long index, long end, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    await db.ListTrimAsync(key, index, end);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public async Task<T> ListGetByIndexAsync<T>(string key, long index, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = await db.ListGetByIndexAsync(key, index);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public async Task<IList<T>> ListRangeAsync<T>(string key, long start, long end, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    var result = await db.ListRangeAsync(key, start, end);
                    return result.ToStringArray().ToObjectNotNullOrEmpty<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public async Task ListInsertBeforeAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        await db.ListInsertBeforeAsync(key, value.SafeString(), insertvalue);
                    else
                        await db.ListInsertBeforeAsync(key, value.ToJson(), insertvalue);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public async Task ListInsertAfterAsync<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        await db.ListInsertAfterAsync(key, value.SafeString(), insertvalue);
                    else
                        await db.ListInsertAfterAsync(key, value.ToJson(), insertvalue);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public async Task<long> ListRemoveAsync<T>(string key, T value, long removecount, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null || value.GetType() == typeof(string))
                        return await db.ListRemoveAsync(key, value.SafeString(), removecount);
                    else
                        return await db.ListRemoveAsync(key, value.ToJson(), removecount);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }
    }
}
