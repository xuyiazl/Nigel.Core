using Microsoft.Extensions.DependencyInjection;

namespace Nigel.Core.AccessControl
{

    public interface IAccessControlBuilder
    {
        IServiceCollection Services { get; }
    }

    internal sealed class AccessControlBuilder : IAccessControlBuilder
    {
        public IServiceCollection Services { get; }

        public AccessControlBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
