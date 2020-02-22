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
    public abstract partial class StackExchangeRedis : IListRedisCommand
    {
        public long ListLeftPushWhenExists<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return db.ListLeftPush(key, value.SafeString(), When.Exists, CommandFlags.None);
                    else
                        return db.ListLeftPush(key, value.ToJson(), When.Exists, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public long ListLeftPush<T>(string key, List<T> value, string connectionName = null)
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
                        return db.ListLeftPush(key, values, CommandFlags.None);
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public long ListLeftPushWhenNoExists<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return db.ListLeftPush(key, value.SafeString(), When.Always, CommandFlags.None);
                    else
                        return db.ListLeftPush(key, value.ToJson(), When.Always, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public long ListRightPushWhenExists<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return db.ListRightPush(key, value.SafeString(), When.Exists, CommandFlags.None);
                    else
                        return db.ListRightPush(key, value.ToJson(), When.Exists, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public long ListRightPushWhenNoExists<T>(string key, T value, string connectionName = null)
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
                        result = db.ListRightPush(key, value.SafeString(), When.Always, CommandFlags.None);
                    else
                        result = db.ListRightPush(key, value.ToJson(), When.Always, CommandFlags.None);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return result;
        }

        public long ListRightPush<T>(string key, List<T> value, string connectionName = null)
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
                        return db.ListRightPush(key, values, CommandFlags.None);
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public T ListLeftPop<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = db.ListLeftPop(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public T ListRightPop<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = db.ListRightPop(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public long ListLength(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    return db.ListLength(key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return 0;
        }

        public void ListSetByIndex<T>(string key, long index, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        db.ListSetByIndex(key, index, value.SafeString());
                    else
                        db.ListSetByIndex(key, index, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public void ListTrim(string key, long index, long end, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    db.ListTrim(key, index, end);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public T ListGetByIndex<T>(string key, long index, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = db.ListGetByIndex(key, index);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public IList<T> ListRange<T>(string key, long start, long end, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    var result = db.ListRange(key, start, end);
                    return result.ToStringArray().ToObjectNotNullOrEmpty<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public void ListInsertBefore<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        db.ListInsertBefore(key, value.SafeString(), insertvalue);
                    else
                        db.ListInsertBefore(key, value.ToJson(), insertvalue);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public void ListInsertAfter<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        db.ListInsertAfter(key, value.SafeString(), insertvalue);
                    else
                        db.ListInsertAfter(key, value.ToJson(), insertvalue);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public long ListRemove<T>(string key, T value, long removecount, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null || value.GetType() == typeof(string))
                        return db.ListRemove(key, value.SafeString(), removecount);
                    else
                        return db.ListRemove(key, value.ToJson(), removecount);
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
