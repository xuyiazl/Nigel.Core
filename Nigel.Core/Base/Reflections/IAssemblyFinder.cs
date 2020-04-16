using Nigel.Core.Dependency;
using Nigel.Core.Finders;
using System.Reflection;

namespace Nigel.Core.Reflections
{
    /// <summary>
    /// 定义程序集查找器
    /// </summary>
    [IgnoreDependency]
    public interface IAssemblyFinder : IFinder<Assembly>
    {
    }
}
