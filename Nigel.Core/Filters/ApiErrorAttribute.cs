﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Nigel.Core.Extensions;
using Nigel.Core.Properties;
using Nigel.Extensions;
using Nigel.Helpers;
using System;

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
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new Result(StateCode.Fail, "", context.Exception.Message);
            }
            else
            {
                var logger = Web.GetService<ILogger<ApiErrorAttribute>>();

                if (logger.IsEnabled(LogLevel.Error))
                {
                    var areaName = context.GetAreaName();
                    var controllerName = context.GetControllerName();
                    var actionName = context.GetActionName();

                    Str str = new Str();
                    str.AppendLine("WebApi全局异常");
                    str.AppendLine($"路由位置：{areaName}{controllerName}/{actionName}");
                    str.AppendLine($"IP：{context.HttpContext.Connection.RemoteIpAddress.ToString()}");
                    str.AppendLine($"请求方法：{context.HttpContext.Request.Method}");
                    str.AppendLine($"请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                    logger.LogError(context.Exception.FormatMessage(str.ToString()));
                }

                context.Result = new Result(StateCode.Fail, "", R.SystemError);

                //context.Result = new RedirectResult(_appSettings.CustomErrorPage);

                //EmailHelper.SendMail(exception);//发送邮件通知到相关人员

                base.OnException(context);
            }
        }
    }
}