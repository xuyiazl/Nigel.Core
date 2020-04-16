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
                return db.ListLeftPush(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public long ListLeftPush<T>(string key, List<T> value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value != null) return 0;
                RedisValue[] values = new RedisValue[value.Count];

                for (int i = 0; i < value.Count; i++)
                    values[i] = redisSerializer.Serializer(value[i]);

                return db.ListLeftPush(key, values, CommandFlags.None);
            });
        }

        public long ListLeftPushWhenNoExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListLeftPush(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
            });
        }

        public long ListRightPushWhenExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListRightPush(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public long ListRightPushWhenNoExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListRightPush(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
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
                    values[i] = redisSerializer.Serializer(value[i]);
                }
                return db.ListRightPush(key, values, CommandFlags.None);
            });
        }

        public T ListLeftPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.ListLeftPop(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public T ListRightPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.ListRightPop(key);
                return redisSerializer.Deserialize<T>(value);
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
                db.ListSetByIndex(key, index, redisSerializer.Serializer(value));
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
                var value = db.ListGetByIndex(key, index);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public IList<T> ListRange<T>(string key, long start, long end, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var result = db.ListRange(key, start, end);
                return redisSerializer.Deserialize<T>(result);
            });
        }

        public long ListInsertBefore<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListInsertBefore(key, redisSerializer.Serializer(value), insertvalue);
            });
        }

        public long ListInsertAfter<T>(string key, T value, string insertvalue, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListInsertAfter(key, redisSerializer.Serializer(value), insertvalue);
            });
        }

        public long ListRemove<T>(string key, T value, long removecount, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.ListRemove(key, redisSerializer.Serializer(value), removecount);
            });
        }
    }
}
