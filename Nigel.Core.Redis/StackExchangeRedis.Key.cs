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
    public abstract partial class StackExchangeRedis : IKeyRedisCommand
    {
        public bool IsKeyExists(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.KeyExists(key);
            });
        }

        public byte[] GetKeyDump(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.KeyDump(key);
            });
        }

        public bool SetKeyExpire(string key, int seconds, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.KeyExpire(key, TimeSpan.FromSeconds(seconds));
            });
        }

        public bool KeyDelete(string Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.KeyDelete(Key);
            });
        }

        public bool KeyPersist(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.KeyPersist(key);
            });
        }
    }
}
