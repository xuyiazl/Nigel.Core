using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    /// <summary>
    /// 缓存操作仓库实现
    /// </summary>
    public class RedisServiceProvider : StackExchangeRedisProvider, IRedisService
    {
        public RedisServiceProvider(IConfiguration configuration, IRedisSerializer redisSerializer)
            : base(configuration, redisSerializer)
        {
        }
    }
}
