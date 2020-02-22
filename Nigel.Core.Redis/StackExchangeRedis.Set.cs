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
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return db.SetAdd(key, value.SafeString());
                    else
                        return db.SetAdd(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public string[] SetMembers(string key, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    var Redisresult = db.SetMembers(key);
                    return Redisresult.ToStringArray();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return null;
        }

        public bool SetExists<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return db.SetContains(key, value.SafeString());
                    else
                        return db.SetContains(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public bool SetRemove<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return db.SetRemove(key, value.SafeString());
                    else
                        return db.SetRemove(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public T SetPop<T>(string key, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();

                    string value = db.SetPop(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return default;
        }

        public long GetSetLength(string key, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return db.SetLength(key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public T SetRandom<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();

                    string value = db.SetRandomMember(key);

                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return default;
        }
    }
}
