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
    public abstract partial class StackExchangeRedis : ILockRedisCommand
    {

        public bool LockExtend<T>(string key, T value, int seconds, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.LockExtend(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

        public string LockQuery(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.LockQuery(key);
            });
        }

        public bool LockRelease<T>(string key, T value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
          {
              return db.LockRelease(key, redisSerializer.Serializer(value));
          });
        }

        public bool LockTake<T>(string key, T value, int seconds, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
          {
              return db.LockTake(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
          });
        }
    }
}
