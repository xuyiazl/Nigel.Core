using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Nigel.Extensions;
using Nigel.Json;
using Nigel.Core.Redis.RedisCommand;
using Nigel.Helpers;

namespace Nigel.Core.Redis
{
    public abstract partial class StackExchangeRedis : IStringRedisCommand
    {
        public TResult StringGetOrInsert<TResult>(string key, Func<TResult> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null)
        {
            if (!KeyExists(key, connectionRead))
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

        public TResult StringGetOrInsert<T, TResult>(string key, Func<T, TResult> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null)
        {
            if (!KeyExists(key, connectionRead))
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

        public bool StringSet<T>(string key, T value, int seconds = 0, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (seconds > 0)
                    return db.StringSet(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                else
                    return db.StringSet(key, redisSerializer.Serializer(value));
            });
        }

        public long StringIncrement(string key, long value = 1, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.StringIncrement(key, value);
            });
        }

        public TResult StringGet<TResult>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.StringGet(key);

                return redisSerializer.Deserialize<TResult>(value);
            });
        }

        public IList<TResult> StringGet<TResult>(string[] keys, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                RedisKey[] redisKey = new RedisKey[keys.Length];
                for (int i = 0; i < redisKey.Length; i++)
                {
                    redisKey[i] = keys[i];
                }

                var redisValue = db.StringGet(redisKey);

                return redisSerializer.Deserialize<TResult>(redisValue);
            });
        }
    }
}
