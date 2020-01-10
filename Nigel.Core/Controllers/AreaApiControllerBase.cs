using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Filters;
using Nigel.Core.Properties;
using System;
using System.Collections.Generic;
using System.Text;

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
