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
        public bool HashSet<T>(string hasId, string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
             {
                 if (value == null) return false;
                 if (value.GetType() == typeof(string))
                     return db.HashSet(hasId, key, value.SafeString());
                 else
                     return db.HashSet(hasId, key, value.ToJson());
             });
        }

        public bool HashSet<T>(string hashId, string Key, T value, OverWrittenTypeDenum isAlways = OverWrittenTypeDenum.Always, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
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
                    return db.HashSet(hashId, Key, value.SafeString(), when);
                else
                    return db.HashSet(hashId, Key, value.ToJson(), when);
            });
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
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                string obj = db.HashGet(hashId, key);
                if (obj == null) return default;
                return obj.SafeString().ToObject<TResult>();
            });
        }

        public IList<TResult> HashGet<TResult>(string hashId, string[] keys, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                List<RedisValue> listvalues = new List<RedisValue>();
                foreach (var key in keys)
                {
                    listvalues.Add(key);
                }
                var value = db.HashGet(hashId, listvalues.ToArray());

                if (value == null) return default;

                var json = value.ToStringArray().ToJsonNotNullOrEmpty();

                return json.ToObject<IList<TResult>>();
            });
        }

        public Dictionary<string, string> HashGetAll(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.HashGetAll(hashId);
                return value.ToStringDictionary();
            });
        }

        public TResult HashGetAll<TResult>(string hashId, string connectionRead, Func<Dictionary<string, string>, TResult> fetcher)
        {
            var obj = HashGetAll(hashId, connectionRead);
            if (obj == null) return default(TResult);

            return fetcher(obj);
        }

        public IList<string> HashKeys(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.HashKeys(hashId);
                return value.ToStringArray().ToList();
            });
        }

        public IList<string> HashValues(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.HashValues(hashId);
                return value.ToStringArray().ToList();
            });
        }

        public IList<TResult> HashValues<TResult>(string hashId, string connectionName = null)
        {
            var value = HashValues(hashId, connectionName);
            return value.ToJsonNotNullOrEmpty().ToObject<IList<TResult>>();
        }

        public bool HashDelete(string hashId, string Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.HashDelete(hashId, Key);
            });
        }

        public long HashDelete(string hashId, string[] Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                RedisValue[] redisKeys = new RedisValue[Key.Length];
                for (int i = 0; i < Key.Length; i++)
                {
                    redisKeys[i] = Key[i];
                }
                return db.HashDelete(hashId, redisKeys);
            });
        }

        public bool HashExists(string hashId, string Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.HashExists(hashId, Key);
            });
        }

        public long HashLength(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.HashLength(hashId);
            });
        }
    }
}
