using Microsoft.Extensions.DependencyInjection;

namespace Nigel.Core.Dependency
{
    /// <summary>
    /// 实现此接口的类型将注册为<see cref="ServiceLifetime.Scoped"/>模式
    /// </summary>
    [IgnoreDependency]
    public interface IScopedDependency
    {
    }
}
