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
    public abstract partial class StackExchangeRedis : IStackExchangeRedis
    {

        /// <summary>
        /// 应用程序的配置接口
        /// </summary>
        private IConfiguration configuration { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="configuration"></param>
        public StackExchangeRedis(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region  获得配置，根据需求可以再次封装
        /// <summary>
        /// 延迟加载配置
        /// </summary>
        protected static Lazy<List<StackExchangeConnectionSettings>> lazyconnMultiplexer;

        private void ThrowExceptions(StackExchangeConnectionSettings config, Exception ex)
        {
            throw new RedisException(config.EndPoint, config.Port, ex.Message, "缓存服务器意外终止,请检查缓存服务器并且将缓存服务器打开");
        }


        private StackExchangeConnectionSettings GetWriteConnection(string connectionName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionName))
                {
                    return lazyconnMultiplexer.Value.Where(a => a.ConnectType == ConnectTypeEnum.Write || a.ConnectType == ConnectTypeEnum.ReadAndWrite).FirstOrDefault();
                }
                else
                {
                    return lazyconnMultiplexer.Value.Where(a => (a.ConnectType == ConnectTypeEnum.Write || a.ConnectType == ConnectTypeEnum.ReadAndWrite) && a.ConnectionName == connectionName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //如果触发异常
                throw new Exception(ex.Message);
            }
        }

        private StackExchangeConnectionSettings GetReadConnection(string connectionName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionName))
                {
                    return lazyconnMultiplexer.Value.Where(a => a.ConnectType == ConnectTypeEnum.Read).FirstOrDefault();
                }
                else
                {
                    return lazyconnMultiplexer.Value.Where(a => a.ConnectType == ConnectTypeEnum.Read && a.ConnectionName == connectionName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //如果触发异常
                throw new Exception(ex.Message);
            }
        }

        private TResult ExecuteCommand<TResult>(ConnectTypeEnum connectTypeEnum, string connectionName, Func<IDatabase, TResult> callback)
        {
            var connect = GetConnection(connectTypeEnum, connectionName);

            if (connect != null)
            {
                try
                {
                    var db = connect.Multiplexer.GetDatabase();
                    return callback.Invoke(db);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(connect, ex);
                }
            }
            return default;
        }

        private void ExecuteCommand(ConnectTypeEnum connectTypeEnum, string connectionName, Action<IDatabase> callback)
        {
            var connect = GetConnection(connectTypeEnum, connectionName);

            if (connect != null)
            {
                try
                {
                    var db = connect.Multiplexer.GetDatabase();
                    callback.Invoke(db);
                }
                catch (Exception ex)
                {
                    ThrowExceptions(connect, ex);
                }
            }
        }

        private StackExchangeConnectionSettings GetConnection(ConnectTypeEnum connectTypeEnum, string connectionName)
        {
            switch (connectTypeEnum)
            {
                case ConnectTypeEnum.Read:
                    return GetReadConnection(connectionName);
                    break;
                case ConnectTypeEnum.Write:
                    return GetWriteConnection(connectionName);
                    break;
                default:
                    return GetWriteConnection(connectionName);
                    break;
            }
        }

        #endregion


        public IDatabase QueryDataBase(ConnectTypeEnum connectTypeEnum, string connectionName = null)
        {
            var config = GetConnection(connectTypeEnum, connectionName);

            return config.Multiplexer.GetDatabase();
        }

        public ISubscriber QuerySubscriber(ConnectTypeEnum connectTypeEnum, string connectionName = null)
        {
            var config = GetConnection(connectTypeEnum, connectionName);

            return config.Multiplexer.GetSubscriber();
        }

        public ServerCounters QueryServerCounters(ConnectTypeEnum connectTypeEnum, string connectionName = null)
        {
            var config = GetConnection(connectTypeEnum, connectionName);

            return config.Multiplexer.GetCounters();
        }

        public ConnectionMultiplexer QueryMultiplexer(ConnectTypeEnum connectTypeEnum, string connectionName = null)
        {
            var config = GetConnection(connectTypeEnum, connectionName);

            return config.Multiplexer;
        }
    }
}