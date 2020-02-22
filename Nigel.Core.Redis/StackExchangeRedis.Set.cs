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
    public abstract partial class StackExchangeRedis : ISetRedisCommand
    {
        public bool SetAdd<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return db.SetAdd(key, value.SafeString());
                else
                    return db.SetAdd(key, value.ToJson());
            });
        }

        public string[] SetMembers(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var Redisresult = db.SetMembers(key);
                return Redisresult.ToStringArray();
            });
        }

        public bool SetExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return db.SetContains(key, value.SafeString());
                else
                    return db.SetContains(key, value.ToJson());
            });
        }

        public bool SetRemove<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return db.SetRemove(key, value.SafeString());
                else
                    return db.SetRemove(key, value.ToJson());
            });
        }

        public T SetPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                string value = db.SetPop(key);
                return value.ToObject<T>();
            });
        }

        public long GetSetLength(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.SetLength(key);
            });
        }

        public T SetRandom<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                string value = db.SetRandomMember(key);

                return value.ToObject<T>();
            });
        }
    }
}
