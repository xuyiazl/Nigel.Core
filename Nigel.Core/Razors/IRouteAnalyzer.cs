using System.Collections.Generic;

namespace Nigel.Core.Razors
{
    /// <summary>
    /// 路由分析器
    /// </summary>
    public interface IRouteAnalyzer
    {
        /// <summary>
        /// 获取所有路由信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<RouteInformation> GetAllRouteInformations();
    }
}
