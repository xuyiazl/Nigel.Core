using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Nigel.Core.HttpFactory;
using Nigel.Core.Razors;
using Nigel.Core.Uploads;
using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Nigel.Core.Extensions
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 绑定获取配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        public static TOptions BindConfigurationSection<TOptions>(this IServiceCollection services, IConfiguration configuration, string section) where TOptions : class, new()
        {
            if (configuration != null && !string.IsNullOrEmpty(section))
            {
                try
                {
                    TOptions t = new TOptions();

                    //需要引用Microsoft.Extensions.Configuration.Binder 组件
                    configuration.GetSection(section).Bind(t);

                    services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);
                    return t;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 注册上传服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddUploadService(this IServiceCollection services)
        {
            services.AddUploadService<DefaultFileUploadService>();
        }

        /// <summary>
        /// 注册上传服务
        /// </summary>
        /// <typeparam name="TFileUploadService">文件上传服务类型</typeparam>
        /// <param name="services">服务集合</param>
        public static void AddUploadService<TFileUploadService>(this IServiceCollection services)
            where TFileUploadService : class, IFileUploadService
        {
            services.TryAddScoped<IFileUploadService, TFileUploadService>();
        }

        /// <summary>
        /// 注册Razor静态Html生成器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRazorHtml(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IRouteAnalyzer, RouteAnalyzer>();
            services.AddScoped<IRazorHtmlGenerator, DefaultRazorHtmlGenerator>();
            return services;
        }

        #region 注册HttpFactory

        private static HttpClientHandler CreateClientHandler()
        {
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            handler.UseDefaultCredentials = false;
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
            return handler;
        }

        /// <summary>
        /// 注册 HttpFactory Service
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            string clientName,
            string baseAddress)
            where TImplementation : class, IHttpService
        {
            services.AddHttpService<TImplementation>(clientName, c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }, TimeSpan.FromSeconds(6), ServiceLifetime.Singleton);

            return services;
        }

        /// <summary>
        /// 注册 HttpFactory Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="configureClient"></param>
        /// <param name="httpClientLeftTime"></param>
        /// <param name="serviceLifetime"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            string clientName,
            Action<HttpClient> configureClient,
            TimeSpan httpClientLeftTime,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TImplementation : class, IHttpService
        {
            services.AddHttpService<TImplementation>(
                new List<KeyValuePair<string, Action<HttpClient>>>() {
                    new KeyValuePair<string, Action<HttpClient>>(clientName,configureClient)
                }, CreateClientHandler, httpClientLeftTime, serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HttpFactory Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="keyValuePair"></param>
        /// <param name="func"></param>
        /// <param name="httpClientLeftTime"></param>
        /// <param name="serviceLifetime"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            IEnumerable<KeyValuePair<string, Action<HttpClient>>> keyValuePair,
            Func<HttpMessageHandler> func,
            TimeSpan httpClientLeftTime,
            ServiceLifetime serviceLifetime)
            where TImplementation : class, IHttpService
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddPolicyRegistry();

            keyValuePair.ForEach(item =>
            {
                services.AddHttpClient(item.Key, item.Value)
                    .ConfigurePrimaryHttpMessageHandler(func)
                    .SetHandlerLifetime(httpClientLeftTime);
            });

            services.AddHttpService<TImplementation>(serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="httpClientLeftTime"></param>
        /// <param name="clientName"></param>
        /// <param name="serviceLifetime"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            TimeSpan httpClientLeftTime,
            string clientName = "apiClient",
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TImplementation : class, IHttpService
        {
            services.AddHttpService<TImplementation>(clientName, CreateClientHandler, httpClientLeftTime, serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="func"></param>
        /// <param name="httpClientLeftTime"></param>
        /// <param name="serviceLifetime"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            string clientName,
            Func<HttpMessageHandler> func,
            TimeSpan httpClientLeftTime,
            ServiceLifetime serviceLifetime)
            where TImplementation : class, IHttpService
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddPolicyRegistry();

            services.AddHttpClient(clientName, client =>
                {
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .ConfigurePrimaryHttpMessageHandler(func)
                .SetHandlerLifetime(httpClientLeftTime);

            services.AddHttpService<TImplementation>(serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        private static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TImplementation : class, IHttpService
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IHttpService, TImplementation>();
                    break;

                case ServiceLifetime.Transient:
                    services.TryAddTransient<IHttpService, TImplementation>();
                    break;

                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IHttpService, TImplementation>();
                    break;
            }

            return services;
        }

        #endregion 注册HttpFactory
    }
}