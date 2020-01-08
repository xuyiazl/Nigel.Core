﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.Middlewares
{
    /// <summary>
    /// 错误日志中间件
    /// </summary>
    public class ErrorLogMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="ErrorLogMiddleware"/>类型的实例
        /// </summary>
        /// <param name="next">方法</param>
        public ErrorLogMiddleware(RequestDelegate next, ILogger<ErrorLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                WriteLog(context, ex);
                throw;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="ex">异常</param>
        private void WriteLog(HttpContext context, Exception ex)
        {
            if (context == null)
                return;

            _logger.LogError($"全局异常捕获 - 错误日志中间件 - 状态码：{context.Response.StatusCode}", ex);
        }
    }
}
