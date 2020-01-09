﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Nigel.Core.Extensions;
using Nigel.Core.Properties;

namespace Nigel.Core.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandlerAttribute> _logger;

        public ExceptionHandlerAttribute(ILogger<ExceptionHandlerAttribute> logger)
        {
            _logger = logger;
        }

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

                if (_logger.IsEnabled(LogLevel.Error))
                {
                    var areaName = context.GetAreaName();
                    var controllerName = context.GetControllerName();
                    var actionName = context.GetActionName();

                    var msg = $"全局异常捕获:{areaName}{controllerName}/{actionName}";
                    _logger.LogError(msg, context.Exception);
                }

                context.Result = new Result(StateCode.Fail, "", R.SystemError);

                //context.Result = new RedirectResult(_appSettings.CustomErrorPage);

                //EmailHelper.SendMail(exception);//发送邮件通知到相关人员

                base.OnException(context);
            }
        }
    }
}
