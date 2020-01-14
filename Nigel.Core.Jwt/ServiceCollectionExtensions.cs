﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Nigel.Core.Jwt
{
    /// <summary>
    /// Extension methods for setting up JWT authentication/authorization middleware in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds authentication/authorization using JWT to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddJwtOptions(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services;
        }

        /// <summary>
        /// Adds authentication/authorization using JWT to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configureOptions">An <see cref="Action{JwtOptions}"/> to configure the provided <see cref="JwtOptions"/>.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddJwtOptions(this IServiceCollection services, Action<JwtOptions> configureOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            services.Configure(configureOptions);

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtOptions>>().Value);

            return services;
        }
    }
}