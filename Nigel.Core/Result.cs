using Microsoft.AspNetCore.Mvc;
using Nigel.Extensions;
using System;
using System.Threading.Tasks;

namespace Nigel.Core
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result : JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public string SubCode { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; }

        /// <summary>
        /// 请求耗时
        /// </summary>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 初始化一个<see cref="Result"/>类型的实例
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(int code, string subCode, string message, dynamic data = null) : base(null)
        {
            Code = code;
            SubCode = subCode;
            Message = message;
            Data = data;
            OperationTime = DateTime.Now;
            ElapsedTime = -1;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(StateCode code, string subCode, string message, dynamic data = null) : base(null)
        {
            Code = code.Value();
            SubCode = subCode;
            Message = message;
            Data = data;
            OperationTime = DateTime.Now;
            ElapsedTime = -1;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this.Value = new
            {
                Code,
                SubCode,
                Message,
                ElapsedTime,
                OperationTime,
                Data
            };
            return base.ExecuteResultAsync(context);
        }
    }
}