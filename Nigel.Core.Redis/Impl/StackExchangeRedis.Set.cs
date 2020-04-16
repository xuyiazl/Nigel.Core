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
                return db.SetAdd(key, redisSerializer.Serializer(value));
            });
        }

        public IList<T> SetMembers<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.SetMembers(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public bool SetExists<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.SetContains(key, redisSerializer.Serializer(value));
            });
        }

        public bool SetRemove<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.SetRemove(key, redisSerializer.Serializer(value));
            });
        }

        public T SetPop<T>(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                var value = db.SetPop(key);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public long SetLength(string key, string connectionName = null)
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
                var value = db.SetRandomMember(key);

                return redisSerializer.Deserialize<T>(value);
            });
        }
    }
}
