using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Nigel.Extensions;
using Nigel.Json;
using Nigel.Core.Redis.RedisCommand;

namespace Nigel.Core.Redis
{
    public abstract partial class StackExchangeRedis : IHashRedisCommand
    {
        public void HashSet<T>(string hasId, string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                        db.HashSet(hasId, key, value.SafeString());
                    else
                        db.HashSet(hasId, key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public bool HashSet<T>(string hashId, string Key, T value, OverWrittenTypeDenum isAlways = OverWrittenTypeDenum.Always, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    When when = When.Always;
                    switch (isAlways)
                    {
                        case OverWrittenTypeDenum.Always:
                            when = When.Always;
                            break;
                        case OverWrittenTypeDenum.Exists:
                            when = When.Exists;
                            break;
                        case OverWrittenTypeDenum.NotExists:
                            when = When.NotExists;
                            break;
                    }

                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        db.HashSet(hashId, Key, value.SafeString(), when);
                    else
                        db.HashSet(hashId, Key, value.ToJson(), when);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public TResult HashGetOrInsert<TResult>(string hashId, string key, string connectionRead, string connectionWrite, Func<TResult> fetcher)
            => HashGetOrInsert<TResult>(hashId, key, 0, connectionRead, connectionWrite, fetcher);

        public TResult HashGetOrInsert<TResult>(string hashId, string key, int seconds, string connectionRead, string connectionWrite, Func<TResult> fetcher)
        {
            if (!HashExists(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke();
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = IsKeyExists(hashId, connectionRead);

                        HashSet(hashId, key, source, connectionWrite);
                        if (!exists)
                        {
                            SetKeyExpire(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        HashSet(hashId, key, source, connectionWrite);
                    }
                }
                return source;
            }
            else
            {
                return HashGet<TResult>(hashId, key, connectionRead);
            }
        }

        public TResult HashGetOrInsert<T, TResult>(string hashId, string key, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
            => HashGetOrInsert<T, TResult>(hashId, key, 0, connectionRead, connectionWrite, fetcher, t);

        public TResult HashGetOrInsert<T, TResult>(string hashId, string key, int seconds, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
        {
            if (!HashExists(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = IsKeyExists(hashId, connectionRead);
                        HashSet(hashId, key, source, connectionWrite);
                        if (!exists)
                        {
                            SetKeyExpire(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        HashSet(hashId, key, source, connectionWrite);
                    }
                }
                return source;
            }
            else
            {
                return HashGet<TResult>(hashId, key, connectionRead);
            }
        }

        public TResult HashGet<TResult>(string hashId, string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string obj = db.HashGet(hashId, key);
                    if (obj == null) return default;
                    return obj.SafeString().ToObject<TResult>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public IList<TResult> HashGet<TResult>(string hashId, string[] keys, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    List<RedisValue> listvalues = new List<RedisValue>();
                    foreach (var key in keys)
                    {
                        listvalues.Add(key);
                    }
                    var value = db.HashGet(hashId, listvalues.ToArray());

                    if (value == null) return default;

                    var json = value.ToStringArray().ToJsonNotNullOrEmpty();

                    return json.ToObject<IList<TResult>>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }

            return default;
        }

        public Dictionary<string, string> HashGetAll(string hashId, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    var value = db.HashGetAll(hashId);
                    return value.ToStringDictionary();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return null;
        }

        public TResult HashGetAll<TResult>(string hashId, string connectionRead, Func<Dictionary<string, string>, TResult> fetcher)
        {
            var obj = HashGetAll(hashId, connectionRead);
            if (obj == null) return default(TResult);

            return fetcher(obj);
        }

        public IList<string> HashKeys(string hashId, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    //value = db.HashGetAll(hashId).ToStringDictionary();
                    var value = db.HashKeys(hashId);
                    return value.ToStringArray().ToList();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return null;
        }

        public IList<string> HashValues(string hashId, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    //value = db.HashGetAll(hashId).ToStringDictionary();
                    var value = db.HashValues(hashId);
                    return value.ToStringArray().ToList();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return null;
        }

        public IList<TResult> HashValues<TResult>(string hashId, string connectionName = null)
        {
            var value = HashValues(hashId, connectionName);
            return value.ToJsonNotNullOrEmpty().ToObject<IList<TResult>>();
        }

        public bool HashDelete(string hashId, string Key, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return db.HashDelete(hashId, Key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public long HashDelete(string hashId, string[] Key, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (Key != null)
                    {
                        RedisValue[] redisKeys = new RedisValue[Key.Length];
                        for (int i = 0; i < Key.Length; i++)
                        {
                            redisKeys[i] = Key[i];
                        }
                        return db.HashDelete(hashId, redisKeys);
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public bool HashExists(string hashId, string Key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    return db.HashExists(hashId, Key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return false;

        }

        public long HashLength(string hashId, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    return db.HashLength(hashId);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return 0;
        }
    }
}
