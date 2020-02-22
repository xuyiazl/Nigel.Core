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
    public abstract partial class StackExchangeRedis : ISortSetRedisCommandAsync
    {
        public async Task<bool> SortedAddAsync<T>(string key, T value, double score, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return false;
                if (value.GetType() == typeof(string))
                    return await db.SortedSetAddAsync(key, value.SafeString(), score);
                else
                    return await db.SortedSetAddAsync(key, value.ToJson(), score);
            });
        }

        public async Task<long> SortedAddAsync<T>(string key, Dictionary<T, double> values, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                List<SortedSetEntry> sortedEntry = new List<SortedSetEntry>();
                foreach (var keyvalue in values)
                {
                    var entry = new SortedSetEntry(keyvalue.Key.ToJson(), keyvalue.Value);
                    sortedEntry.Add(entry);
                }
                return await db.SortedSetAddAsync(key, sortedEntry.ToArray());
            });
        }

        public async Task<long> SortedCountAsync(string key, double start, double end, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.SortedSetLengthAsync(key, start, end);
            });
        }

        public async Task<long> SortedRemoveAsync<T>(string key, IList<T> values, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                List<RedisValue> listValues = new List<RedisValue>();
                foreach (var val in values)
                {
                    listValues.Add(val.ToJson());
                }

                return await db.SortedSetRemoveAsync(key, listValues.ToArray());
            });
        }

        public async Task<bool> SortedRemoveAsync<T>(string key, T value, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (value == null) return false;
                return await db.SortedSetRemoveAsync(key, value.ToJson());
            });
        }

        public async Task<long> SortedRemoveAsync(string key, long start, long stop, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.SortedSetRemoveRangeByRankAsync(key, start, stop);
            });
        }

        public async Task<IList<T>> SortedRangeByScoreAsync<T>(string key, double start, double stop, int orderby = 0, int skip = 0, int take = -1, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                Order o = orderby == 1 ? Order.Descending : Order.Ascending;
                var resultEntry = await db.SortedSetRangeByScoreAsync(key, start, stop, order: o, skip: skip, take: take);

                return resultEntry.Select(t => t.ToString()).ToList().ToObjectNotNullOrEmpty<T>();
            });
        }

        public async Task<Dictionary<T, double>> SortedRangeAsync<T>(string key, long start, long stop, int orderby = 0, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                Order o = orderby == 1 ? Order.Descending : Order.Ascending;
                var resultEntry = await db.SortedSetRangeByRankWithScoresAsync(key, start, stop, order: o);
                return resultEntry.ToDictionary(t => t.Element.SafeString().ToObject<T>(), t => t.Score);
            });
        }

        public async Task<long?> SortedZrankAsync<T>(string key, T value, int orderby = 0, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                Order o = orderby == 1 ? Order.Descending : Order.Ascending;
                if (value == null) return 0;
                if (value.GetType() == typeof(string))
                    return await db.SortedSetRankAsync(key, value.SafeString(), o);
                else
                    return await db.SortedSetRankAsync(key, value.ToJson(), o);
            });
        }
    }
}
