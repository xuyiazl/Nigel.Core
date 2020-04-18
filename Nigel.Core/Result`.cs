using Microsoft.AspNetCore.Mvc;
using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core
{
    /// <summary>
    /// 返回结构体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public string subCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime operationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 请求耗时
        /// </summary>
        public long elapsedTime { get; set; } = -1;
    }
}
