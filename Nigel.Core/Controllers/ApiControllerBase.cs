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
    /// WebApi控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ErrorLog]
    [TraceLog]
    public abstract class ApiControllerBase : Controller
    {
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger _logger;

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="subCode">业务状态码</param>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual IActionResult Success(string subCode, dynamic data = null, string message = null)
        {
            if (message == null)
                message = R.Success;
            return new Result(StateCode.Ok, subCode, message, data);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected IActionResult Fail(string subCode, string message) => new Result(StateCode.Fail, subCode, message);
    }
}
