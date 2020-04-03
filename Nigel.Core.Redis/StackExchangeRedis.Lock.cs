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

        public bool LockExtend(string key, string value, int seconds, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.LockExtend(key, value, TimeSpan.FromSeconds(seconds));
            });
        }

        public string LockQuery(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.LockQuery(key);
            });
        }

        public bool LockRelease(string key, string value, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
          {
              return db.LockRelease(key, value);
          });
        }

        public bool LockTake(string key, string value, int seconds, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
          {
              return db.LockTake(key, value, TimeSpan.FromSeconds(seconds));
          });
        }
    }
}
