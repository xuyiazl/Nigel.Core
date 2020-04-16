using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nigel.Core.Builders;
using Nigel.Helpers;
using Nigel.Core.Modularity;
using Nigel.Core.Options;
using Nigel.Core.Reflections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core
{

    /// <summary>
    /// 服务扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 将Bing服务，各个<see cref="BingModule"/>模块的服务添加到服务容器中
        /// </summary>
        /// <typeparam name="TBingModuleManager">Bing模块管理器类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="builderAction">Bing构建器操作</param>
        public static IServiceCollection AddBing<TBingModuleManager>(this IServiceCollection services,
            Action<IBingBuilder> builderAction = null) where TBingModuleManager : IBingModuleManager, new()
        {
            Check.NotNull(services, nameof(services));

            var configuration = services.GetConfiguration();
            Singleton<IConfiguration>.Instance = configuration;

            // 初始化所有程序集查找器
            services.TryAddSingleton<IAllAssemblyFinder>(new AppDomainAllAssemblyFinder());

            var builder = services.GetSingletonInstanceOrNull<IBingBuilder>() ?? new BingBuilder();
            builderAction?.Invoke(builder);
            services.TryAddSingleton<IBingBuilder>(builder);

            var manager = new TBingModuleManager();
            services.AddSingleton<IBingModuleManager>(manager);
            manager.LoadModules(services);
            return services;
        }

        /// <summary>
        /// 获取Bing框架配置选项信息
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        public static BingOptions GetBingOptions(this IServiceProvider provider) => provider.GetService<IOptions<BingOptions>>()?.Value;

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            var factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="type">指定类型</param>
        public static ILogger GetLogger(this IServiceProvider provider, Type type)
        {
            var factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">名称</param>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            var factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }

        /// <summary>
        /// Bing框架初始化，适用于非AspNetCore环境
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        public static IServiceProvider UseBing(this IServiceProvider provider)
        {
            var moduleManager = provider.GetService<IBingModuleManager>();
            moduleManager.UseModule(provider);
            return provider;
        }
    }
}
