using Nigel.Core.Redis.RedisCommand;
using Nigel.Extensions;
using Nigel.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nigel.Core.Redis
{
    public abstract partial class StackExchangeRedis : IHashRedisCommandAsync
    {
        public async Task<bool> HashSetAsync<T>(string hasId, string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
               {
                   if (value == null) return false;
                   if (value.GetType() == typeof(string))
                       return await db.HashSetAsync(hasId, key, value.SafeString());
                   else
                       return await db.HashSetAsync(hasId, key, value.ToJson());
               });
        }

        public async Task<bool> HashSetAsync<T>(string hashId, string Key, T value, OverWrittenTypeDenum isAlways = OverWrittenTypeDenum.Always, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
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
                     return await db.HashSetAsync(hashId, Key, value.SafeString(), when);
                 else
                     return await db.HashSetAsync(hashId, Key, value.ToJson(), when);
             });
        }

        public async Task<TResult> HashGetOrInsertAsync<TResult>(string hashId, string key, string connectionRead, string connectionWrite, Func<TResult> fetcher)
            => await HashGetOrInsertAsync<TResult>(hashId, key, 0, connectionRead, connectionWrite, fetcher);

        public async Task<TResult> HashGetOrInsertAsync<TResult>(string hashId, string key, int seconds, string connectionRead, string connectionWrite, Func<TResult> fetcher)
        {
            if (!await HashExistsAsync(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke();
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = await IsKeyExistsAsync(hashId, connectionRead);

                        await HashSetAsync(hashId, key, source, connectionWrite);
                        if (!exists)
                        {
                            await SetKeyExpireAsync(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        await HashSetAsync(hashId, key, source, connectionWrite);
                    }
                }
                return source;
            }
            else
            {
                return await HashGetAsync<TResult>(hashId, key, connectionRead);
            }
        }

        public async Task<TResult> HashGetOrInsertAsync<T, TResult>(string hashId, string key, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
            => await HashGetOrInsertAsync<T, TResult>(hashId, key, 0, connectionRead, connectionWrite, fetcher, t);

        public async Task<TResult> HashGetOrInsertAsync<T, TResult>(string hashId, string key, int seconds, string connectionRead, string connectionWrite, Func<T, TResult> fetcher, T t)
        {
            if (!await HashExistsAsync(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = await IsKeyExistsAsync(hashId, connectionRead);
                        await HashSetAsync(hashId, key, source, connectionWrite);
                        if (!exists)
                        {
                            await SetKeyExpireAsync(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        await HashSetAsync(hashId, key, source, connectionWrite);
                    }
                }
                return source;
            }
            else
            {
                return await HashGetAsync<TResult>(hashId, key, connectionRead);
            }
        }

        public async Task<TResult> HashGetAsync<TResult>(string hashId, string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                string obj = await db.HashGetAsync(hashId, key);
                if (obj == null) return default;
                return obj.SafeString().ToObject<TResult>();
            });
        }

        public async Task<IList<TResult>> HashGetAsync<TResult>(string hashId, string[] keys, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                List<RedisValue> listvalues = new List<RedisValue>();
                foreach (var key in keys)
                {
                    listvalues.Add(key);
                }
                var value = await db.HashGetAsync(hashId, listvalues.ToArray());

                if (value == null) return default;

                var json = value.ToStringArray().ToJsonNotNullOrEmpty();

                return json.ToObject<IList<TResult>>();
            });
        }

        public async Task<Dictionary<string, string>> HashGetAllAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.HashGetAllAsync(hashId);
                return value.ToStringDictionary();
            });
        }

        public async Task<TResult> HashGetAllAsync<TResult>(string hashId, string connectionRead, Func<Dictionary<string, string>, TResult> fetcher)
        {
            var obj = await HashGetAllAsync(hashId, connectionRead);
            if (obj == null) return default(TResult);

            return fetcher(obj);
        }

        public async Task<IList<string>> HashKeysAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.HashKeysAsync(hashId);
                return value.ToStringArray().ToList();
            });
        }

        public async Task<IList<string>> HashValuesAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.HashValuesAsync(hashId);
                return value.ToStringArray().ToList();
            });
        }

        public async Task<IList<TResult>> HashValuesAsync<TResult>(string hashId, string connectionName = null)
        {
            var value = await HashValuesAsync(hashId, connectionName);
            return value.ToJsonNotNullOrEmpty().ToObject<IList<TResult>>();
        }

        public async Task<bool> HashDeleteAsync(string hashId, string Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.HashDeleteAsync(hashId, Key);
            });
        }

        public async Task<long> HashDeleteAsync(string hashId, string[] Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] redisKeys = new RedisValue[Key.Length];
                for (int i = 0; i < Key.Length; i++)
                {
                    redisKeys[i] = Key[i];
                }
                return await db.HashDeleteAsync(hashId, redisKeys);
            });
        }

        public async Task<bool> HashExistsAsync(string hashId, string Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.HashExistsAsync(hashId, Key);
            });
        }

        public async Task<long> HashLengthAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.HashLengthAsync(hashId);
            });
        }
    }
}
