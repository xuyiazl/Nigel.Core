using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Nigel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Nigel.Core.Extensions;
using Nigel.Core.Properties;

namespace Nigel.Core.Filters
{
    /// <summary>
    /// API错误日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context">异常上下文</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
                return;

            if (context.Exception is OperationCanceledException)
            {
                //过滤掉客户端取消请求的异常，属于正常异常范围
                //_logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new Result(StateCode.Fail, "", R.CanceledMessage);
            }
            else
            {
                var logger = Web.GetService<ILogger<ApiErrorAttribute>>();

                if (logger.IsEnabled(LogLevel.Error))
                {
                    var areaName = context.GetAreaName();
                    var controllerName = context.GetControllerName();
                    var actionName = context.GetActionName();

                    string message = $"WebApi全局异常 {areaName}{controllerName}/{actionName} - IP：{context.HttpContext.Connection.RemoteIpAddress.ToString()} ， 请求方法：{context.HttpContext.Request.Method}，请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";

                    logger.LogError(context.Exception, message);
                }

                context.Result = new Result(StateCode.Fail, "", R.SystemError);

                //context.Result = new RedirectResult(_appSettings.CustomErrorPage);

                //EmailHelper.SendMail(exception);//发送邮件通知到相关人员

                base.OnException(context);
            }
        }
    }
}
