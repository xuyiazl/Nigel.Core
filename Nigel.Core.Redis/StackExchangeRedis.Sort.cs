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
    public abstract partial class StackExchangeRedis : ISortSetRedisCommand
    {
        public bool SortedAdd<T>(string key, T value, double score, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    if (value.GetType() == typeof(string))
                        return db.SortedSetAdd(key, value.SafeString(), score);
                    else
                        return db.SortedSetAdd(key, value.ToJson(), score);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public long SortedAdd<T>(string key, Dictionary<T, double> values, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    List<SortedSetEntry> sortedEntry = new List<SortedSetEntry>();
                    foreach (var keyvalue in values)
                    {
                        var entry = new SortedSetEntry(keyvalue.Key.ToJson(), keyvalue.Value);
                        sortedEntry.Add(entry);
                    }
                    return db.SortedSetAdd(key, sortedEntry.ToArray());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public long SortedCount(string key, double start, double end, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    return db.SortedSetLength(key, start, end);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return 0;
        }

        public long SortedRemove<T>(string key, IList<T> values, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    List<RedisValue> listValues = new List<RedisValue>();
                    foreach (var val in values)
                    {
                        listValues.Add(val.ToJson());
                    }

                    return db.SortedSetRemove(key, listValues.ToArray());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public bool SortedRemove<T>(string key, T value, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    if (value == null) return false;
                    return db.SortedSetRemove(key, value.ToJson());
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return false;
        }

        public long SortedRemove(string key, long start, long stop, string connectionName = null)
        {
            var writeConn = GetWriteConfig(connectionName);
            if (writeConn != null)
            {
                try
                {
                    var db = writeConn.Multiplexer.GetDatabase();
                    return db.SortedSetRemoveRangeByRank(key, start, stop);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(writeConn, ex);
                }
            }
            return 0;
        }

        public IList<T> SortedRangeByScore<T>(string key, double start, double stop, int orderby = 0, int skip = 0, int take = -1, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    Order o = Order.Ascending;
                    if (orderby == 1)
                    {
                        o = Order.Descending;
                    }
                    var resultEntry = db.SortedSetRangeByScore(key, start, stop, order: o, skip: skip, take: take);

                    return resultEntry.Select(t => t.ToString()).ToList().ToObjectNotNullOrEmpty<T>();

                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return null;
        }

        public Dictionary<T, double> SortedRange<T>(string key, long start, long stop, int orderby = 0, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    var db = readConn.Multiplexer.GetDatabase();
                    Order o = Order.Ascending;
                    if (orderby == 1)
                    {
                        o = Order.Descending;
                    }
                    var resultEntry = db.SortedSetRangeByRankWithScores(key, start, stop, order: o);
                    return resultEntry.ToDictionary(t => t.Element.SafeString().ToObject<T>(), t => t.Score);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }

            return default;
        }

        public long? SortedZrank<T>(string key, T value, int orderby = 0, string connectionName = null)
        {
            var readConn = GetReadConfig(connectionName);
            if (readConn != null)
            {
                try
                {
                    Order o = Order.Ascending;
                    if (orderby == 1)
                    {
                        o = Order.Descending;
                    }
                    var db = readConn.Multiplexer.GetDatabase();
                    if (value == null) return 0;
                    if (value.GetType() == typeof(string))
                        return db.SortedSetRank(key, value.SafeString(), o);
                    else
                        return db.SortedSetRank(key, value.ToJson(), o);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(readConn, ex);
                }
            }
            return 0;
        }
    }
}
