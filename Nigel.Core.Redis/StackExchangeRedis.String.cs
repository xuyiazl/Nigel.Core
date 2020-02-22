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
    public abstract partial class StackExchangeRedis : IStringRedisCommand
    {
        public TResult StringGetOrInsert<TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<TResult> fetcher)
        {
            if (!IsKeyExists(key, connectionRead))
            {
                var source = fetcher.Invoke();
                if (source != null)
                    StringSet(key, source, seconds, connectionWrite);
                return source;
            }
            else
            {
                return StringGet<TResult>(key, connectionRead);
            }
        }

        public TResult StringGetOrInsert<T, TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
        {
            if (!IsKeyExists(key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                    StringSet(key, source, seconds, connectionWrite);
                return source;
            }
            else
            {
                return StringGet<TResult>(key, connectionRead);
            }
        }

        public void StringSet<T>(string key, T value, int seconds = 0, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();

                    if (value == null) return;
                    if (value.GetType() == typeof(string))
                    {
                        if (seconds > 0)
                            db.StringSet(key, value.SafeString(), TimeSpan.FromSeconds(seconds));
                        else
                            db.StringSet(key, value.SafeString());
                    }
                    else
                    {
                        if (seconds > 0)
                            db.StringSet(key, value.ToJson(), TimeSpan.FromSeconds(seconds));
                        else
                            db.StringSet(key, value.ToJson());
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public long StringIncrement(string key, long value = 1, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return db.StringIncrement(key, value);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return -1;
        }

        public TResult StringGet<TResult>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = db.StringGet(key);
                    if (value == null) return default;
                    return value.ToObject<TResult>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }

        public List<TResult> StringGet<TResult>(string[] keys, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    if (keys != null && keys.Length != 0)
                    {
                        RedisKey[] redisKey = new RedisKey[keys.Length];
                        for (int i = 0; i < redisKey.Length; i++)
                        {
                            redisKey[i] = keys[i];
                        }

                        var redisValue = db.StringGet(redisKey);
                        var json = redisValue.ToStringArray().ToJsonNotNullOrEmpty();
                        return json.ToObject<List<TResult>>();
                    }

                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }
    }
}
