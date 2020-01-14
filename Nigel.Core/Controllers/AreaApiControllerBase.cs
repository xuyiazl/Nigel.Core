using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nigel.Core.Controllers
{
    /// <summary>
    /// WebApi的区域控制器基类
    /// </summary>
    [Route("api/[area]/[controller]")]
    public abstract class AreaApiControllerBase : ApiControllerBase
    {
        public AreaApiControllerBase(ILogger logger)
            : base(logger)
        {
        }
    }
}