﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.Redis.RedisCommand
{
    /// <summary>
    /// Redis 有序集合命令
    /// </summary>
    public interface ISortSetRedisCommandAsync
    {
        /// <summary>
        /// 添加有序集和对象,向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <param name="connectionName"></param>
        Task<bool> SortedAddAsync<T>(string key, T value, double score, string connectionName = null);
        /// <summary>
        /// 批量添加有序集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long> SortedAddAsync<T>(string key, Dictionary<T, double> values, string connectionName = null);
        /// <summary>
        /// 获得有续集和范围内的集合数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long> SortedCountAsync(string key, double start, double end, string connectionName = null);
        /// <summary>
        /// 删除有续集和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<bool> SortedRemoveAsync<T>(string key, T value, string connectionName = null);
        /// <summary>
        /// 删除有序集合根据多个values集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long> SortedRemoveAsync<T>(string key, IList<T> values, string connectionName = null);
        /// <summary>
        /// 删除有序集合通过游标
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long> SortedRemoveAsync(string key, long start, long stop, string connectionName = null);
        /// <summary>
        /// 获取有续集和的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="orderby">0：Ascending，1：Descending</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<IList<T>> SortedRangeByScoreAsync<T>(string key, double start, double stop, int orderby = 0, int skip = 0, int take = -1, string connectionName = null);
        /// <summary>
        /// 获取有序集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="orderby">0：Ascending，1：Descending</param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<Dictionary<T, double>> SortedRangeAsync<T>(string key, long start, long stop, int orderby = 0, string connectionName = null);
        /// <summary>
        /// 返回有序集合中指定成员的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="orderby">0：Ascending，1：Descending</param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<long?> SortedZrankAsync<T>(string key, T value, int orderby = 0, string connectionName = null);
    }
}
