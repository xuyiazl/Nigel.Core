﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    /// <summary>
    /// Redis的组件服务提供
    /// </summary>
    public class StackExchangeRedisProvider : StackExchangeRedis, IStackExchangeRedis
    {
        public StackExchangeRedisProvider(IConfiguration configuration) : base(configuration)
        {
            // 读取配置文件中的Redis字符串信息
            if (lazyconnMultiplexer == null || !lazyconnMultiplexer.IsValueCreated)
            {
                lazyconnMultiplexer = new Lazy<List<StackExchangeConnectionSettings>>(() =>
                {
                    var config = new List<StackExchangeConnectionSettings>();
                    configuration.GetSection("RedisConnection").Bind(config);
                    return config;
                });
            }
        }
    }
}
