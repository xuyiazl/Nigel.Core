using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务集合 - 配置 扩展
    /// </summary>
    public static class ServiceCollectionConfigurationExtensions
    {
        /// <summary>
        /// 替换配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static IServiceCollection ReplaceConfiguration(this IServiceCollection services,IConfiguration configuration) => services.Replace(ServiceDescriptor.Singleton(configuration));

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
            if (hostBuilderContext?.Configuration != null)
                return hostBuilderContext.Configuration as IConfigurationRoot;
            return services.GetSingletonInstance<IConfiguration>();
        }

    }
}
