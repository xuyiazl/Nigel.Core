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
    public abstract partial class StackExchangeRedis : ISetRedisCommandAsync
    {
        public async Task<bool> SetAddAsync<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return await db.SetAddAsync(key, value.SafeString());
                    else
                        return await db.SetAddAsync(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public async Task<string[]> SetMembersAsync(string key, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    var Redisresult = await db.SetMembersAsync(key);
                    return Redisresult.ToStringArray();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return null;
        }

        public async Task<bool> SetExistsAsync<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return await db.SetContainsAsync(key, value.SafeString());
                    else
                        return await db.SetContainsAsync(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return await db.SetRemoveAsync(key, value.SafeString());
                    else
                        return await db.SetRemoveAsync(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public async Task<T> SetPopAsync<T>(string key, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();

                    string value = await db.SetPopAsync(key);
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return default;
        }

        public async Task<long> GetSetLengthAsync(string key, string connectionName = null)
        {
            var writeConn = GetReadConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return await db.SetLengthAsync(key);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public async Task<T> SetRandomAsync<T>(string key, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();

                    string value = await db.SetRandomMemberAsync(key);

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
