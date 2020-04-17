﻿using System;
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

        public bool LockExtend<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.LockExtend(key, serializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                return db.LockExtend(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

        public T LockQuery<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                var value = db.LockQuery(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public bool LockRelease<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.LockRelease(key, serializer.Serializer(value));
                return db.LockRelease(key, redisSerializer.Serializer(value));
            });
        }

        public bool LockTake<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.LockTake(key, serializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                return db.LockTake(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }
    }
}
