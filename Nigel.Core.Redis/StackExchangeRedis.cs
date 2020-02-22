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
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="config"></param>
        /// <param name="ex"></param>
        private void ThrowExceptions(StackExchangeConnectionSettings config, Exception ex)
        {
            throw new RedisException(config.EndPoint, config.Port, ex.Message, "缓存服务器意外终止,请检查缓存服务器并且将缓存服务器打开");
        }

        /// <summary>
        /// 获得写入的Redis连接配置
        /// </summary>
        /// <returns></returns>
        private StackExchangeConnectionSettings GetWriteConfig(string connectionName = null)
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

        /// <summary>
        /// 获得读取的Redis连接配置
        /// </summary>
        /// <returns></returns>
        private StackExchangeConnectionSettings GetReadConfig(string connectionName = null)
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

        #endregion


        public IDatabase QueryDataBase(ConnectTypeEnum connect, string connectionName = null)
        {
            StackExchangeConnectionSettings config;
            switch (connect)
            {
                case ConnectTypeEnum.Read:
                    config = GetReadConfig(connectionName);
                    break;
                case ConnectTypeEnum.Write:
                    config = GetWriteConfig(connectionName);
                    break;
                default:
                    config = GetWriteConfig(connectionName);
                    break;
            }
            return config.Multiplexer.GetDatabase();
        }

        public ISubscriber QuerySubscriber(ConnectTypeEnum connect, string connectionName = null)
        {
            StackExchangeConnectionSettings config;
            switch (connect)
            {
                case ConnectTypeEnum.Read:
                    config = GetReadConfig(connectionName);
                    break;
                case ConnectTypeEnum.Write:
                    config = GetWriteConfig(connectionName);
                    break;
                default:
                    config = GetWriteConfig(connectionName);
                    break;
            }
            return config.Multiplexer.GetSubscriber();
        }

        public ServerCounters QueryServerCounters(ConnectTypeEnum connect, string connectionName = null)
        {
            StackExchangeConnectionSettings config;
            switch (connect)
            {
                case ConnectTypeEnum.Read:
                    config = GetReadConfig(connectionName);
                    break;
                case ConnectTypeEnum.Write:
                    config = GetWriteConfig(connectionName);
                    break;
                default:
                    config = GetWriteConfig(connectionName);
                    break;
            }
            return config.Multiplexer.GetCounters();
        }

        public ConnectionMultiplexer QueryMultiplexer(ConnectTypeEnum connect, string connectionName = null)
        {
            StackExchangeConnectionSettings config;
            switch (connect)
            {
                case ConnectTypeEnum.Read:
                    config = GetReadConfig(connectionName);
                    break;
                case ConnectTypeEnum.Write:
                    config = GetWriteConfig(connectionName);
                    break;
                default:
                    config = GetWriteConfig(connectionName);
                    break;
            }
            return config.Multiplexer;
        }
    }
}
