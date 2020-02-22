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
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListLeftPush(key, value.SafeString(), When.Exists, CommandFlags.None);
                else
                    return db.ListLeftPush(key, value.ToJson(), When.Exists, CommandFlags.None);
            });
        }

        public long ListLeftPush<T>(string key, List<T> value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value != null) return 0;
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    values[i] = value[i].ToJson();
                }
                return db.ListLeftPush(key, values, CommandFlags.None);
            });
        }

        public long ListLeftPushWhenNoExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListLeftPush(key, value.SafeString(), When.Always, CommandFlags.None);
                else
                    return db.ListLeftPush(key, value.ToJson(), When.Always, CommandFlags.None);
            });
        }

        public long ListRightPushWhenExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListRightPush(key, value.SafeString(), When.Exists, CommandFlags.None);
                else
                    return db.ListRightPush(key, value.ToJson(), When.Exists, CommandFlags.None);
            });
        }

        public long ListRightPushWhenNoExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListRightPush(key, value.SafeString(), When.Always, CommandFlags.None);
                else
                    return db.ListRightPush(key, value.ToJson(), When.Always, CommandFlags.None);
            });
        }

        public long ListRightPush<T>(string key, List<T> value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value != null) return 0;
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    values[i] = value[i].ToJson();
                }
                return db.ListRightPush(key, values, CommandFlags.None);
            });
        }

        public T ListLeftPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                string value = db.ListLeftPop(key);
                return value.ToObject<T>();
            });
        }

        public T ListRightPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                string value = db.ListRightPop(key);
                return value.ToObject<T>();
            });
        }

        public long ListLength(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.ListLength(key);
            });
        }

        public void ListSetByIndex<T>(string key, long index, T value, string connectionName = null)
        {
            ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value.GetType() == typeof(string))
                    db.ListSetByIndex(key, index, value.SafeString());
                else
                    db.ListSetByIndex(key, index, value.ToJson());
            });
        }

        public void ListTrim(string key, long index, long end, string connectionName = null)
        {
            ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                db.ListTrim(key, index, end);
            });
        }

        public T ListGetByIndex<T>(string key, long index, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                string value = db.ListGetByIndex(key, index);
                return value.ToObject<T>();
            });
        }

        public IList<T> ListRange<T>(string key, long start, long end, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var result = db.ListRange(key, start, end);
                return result.ToStringArray().ToObjectNotNullOrEmpty<T>();
            });
        }

        public long ListInsertBefore<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
             {
                 if (value == null) return 0;
                 if (value.GetType() == typeof(string))
                     return db.ListInsertBefore(key, value.SafeString(), insertvalue);
                 else
                     return db.ListInsertBefore(key, value.ToJson(), insertvalue);
             });
        }

        public long ListInsertAfter<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListInsertAfter(key, value.SafeString(), insertvalue);
                else
                    return db.ListInsertAfter(key, value.ToJson(), insertvalue);
            });
        }

        public long ListRemove<T>(string key, T value, long removecount, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return db.ListRemove(key, value.SafeString(), removecount);
                else
                    return db.ListRemove(key, value.ToJson(), removecount);
            });
        }
    }
}
