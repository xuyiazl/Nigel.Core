using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Nigel.Core;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// Add HttpFactoryMessage
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        static HttpClientHandler CreateClientHandler()
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

            services.AddHttpClient(clientName)
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
        static IServiceCollection AddHttpService<TImplementation>(this IServiceCollection services,
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
    }
}
