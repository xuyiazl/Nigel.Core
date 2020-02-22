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
    public abstract partial class StackExchangeRedis : IStringRedisCommandAsync
    {
        public async Task<TResult> StringGetOrInsertAsync<TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<TResult> fetcher)
        {
            if (!await IsKeyExistsAsync(key, connectionRead))
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

        public async Task<TResult> StringGetOrInsertAsync<T, TResult>(string key, int seconds, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
        {
            if (!await IsKeyExistsAsync(key, connectionRead))
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

        public async Task StringSetAsync<T>(string key, T value, int seconds = 0, string connectionName = null)
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
                            await db.StringSetAsync(key, value.SafeString(), TimeSpan.FromSeconds(seconds));
                        else
                            await db.StringSetAsync(key, value.SafeString());
                    }
                    else
                    {
                        if (seconds > 0)
                            await db.StringSetAsync(key, value.ToJson(), TimeSpan.FromSeconds(seconds));
                        else
                            await db.StringSetAsync(key, value.ToJson());
                    }
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
        }

        public async Task<long> StringIncrementAsync(string key, long value = 1, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return await db.StringIncrementAsync(key, value);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return -1;
        }

        public async Task<TResult> StringGetAsync<TResult>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    string value = await db.StringGetAsync(key);
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

        public async Task<List<TResult>> StringGetAsync<TResult>(string[] keys, string connectionName = null)
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

                        var redisValue = await db.StringGetAsync(redisKey);
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
