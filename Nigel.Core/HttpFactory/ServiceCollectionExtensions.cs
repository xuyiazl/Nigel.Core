using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// Add HttpFactoryMessage
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 HttpFactory Service
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IHttpService
        {

            return services.AddHttpService<TImplementation>(() =>
            {
                var handler = new HttpClientHandler();
                handler.AllowAutoRedirect = false;
                handler.UseDefaultCredentials = false;
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                return handler;

            });
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="func"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="clientName"></param>
        public static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
            Func<HttpMessageHandler> func,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
            string clientName = "apiClient")
            where TImplementation : class, IHttpService
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //使用http请求,定义一个请求别名，请求别名用于获取外部的http接口请求
            services.AddPolicyRegistry();

            services.AddHttpClient(clientName)
                .ConfigurePrimaryHttpMessageHandler(func);

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
    }
}
