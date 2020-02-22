using System;
using System.Collections.Generic;
using System.Text;
using Nigel.Core.Redis.RedisCommand;
using StackExchange.Redis;

namespace Nigel.Core.Redis
{
    /// <summary>
    /// 定义功能操作部分
    /// 详细命令参考:http://redisdoc.com/
    /// </summary>
    public interface IStackExchangeRedis :
        IKeyRedisCommand, IStringRedisCommand, IHashRedisCommand, ISetRedisCommand, ISortSetRedisCommand, IListRedisCommand,
        IKeyRedisCommandAsync, IStringRedisCommandAsync, IHashRedisCommandAsync, ISetRedisCommandAsync, ISortSetRedisCommandAsync, IListRedisCommandAsync
    {
        /// <summary>
        /// 查询返回IDataBase
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        IDatabase QueryDataBase(ConnectTypeEnum connect, string ConnectionName = null);
        /// <summary>
        /// 查询返回ISubscriber
        /// 消息中间件使用
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        ISubscriber QuerySubscriber(ConnectTypeEnum connect, string ConnectionName = null);
        /// <summary>
        /// 查询返回ServerCounters
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        ServerCounters QueryServerCounters(ConnectTypeEnum connect, string ConnectionName = null);
        /// <summary>
        /// 获得连接器
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        ConnectionMultiplexer QueryMultiplexer(ConnectTypeEnum connect, string ConnectionName = null);
    }
}
