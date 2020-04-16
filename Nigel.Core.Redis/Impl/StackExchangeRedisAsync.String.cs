﻿using System;
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
    public abstract partial class StackExchangeRedis : IStringRedisCommandAsync
    {
        public async Task<TResult> StringGetOrInsertAsync<TResult>(string key, Func<TResult> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null)
        {
            if (!await KeyExistsAsync(key, connectionRead))
            {
                var source = fetcher.Invoke();
                if (source != null)
                    await StringSetAsync(key, source, seconds, connectionWrite);
                return source;
            }
            else
            {
                return await StringGetAsync<TResult>(key, connectionRead);
            }
        }

        public async Task<TResult> StringGetOrInsertAsync<T, TResult>(string key, Func<T, TResult> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null)
        {
            if (!await KeyExistsAsync(key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                    await StringSetAsync(key, source, seconds, connectionWrite);
                return source;
            }
            else
            {
                return await StringGetAsync<TResult>(key, connectionRead);
            }
        }

        public async Task<bool> StringSetAsync<T>(string key, T value, int seconds = 0, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (seconds > 0)
                    return await db.StringSetAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                else
                    return await db.StringSetAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<long> StringIncrementAsync(string key, long value = 1, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.StringIncrementAsync(key, value);
            });
        }

        public async Task<TResult> StringGetAsync<TResult>(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.StringGetAsync(key);
                return redisSerializer.Deserialize<TResult>(value);
            });
        }

        public async Task<IList<TResult>> StringGetAsync<TResult>(string[] keys, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                RedisKey[] redisKey = new RedisKey[keys.Length];
                for (int i = 0; i < redisKey.Length; i++)
                {
                    redisKey[i] = keys[i];
                }

                var redisValue = await db.StringGetAsync(redisKey);

                return redisSerializer.Deserialize<TResult>(redisValue);
            });
        }
    }
}
