using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Redis注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddRedisService(this IServiceCollection services)
        {
            services.AddSingleton<IRedisService, RedisServiceProvider>();
        }
    }
}
