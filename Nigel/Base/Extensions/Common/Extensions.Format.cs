﻿using System;
using System.Globalization;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Nigel.Extensions
{
    /// <summary>
    /// 系统扩展 - 格式化扩展
    /// </summary>
    public static partial class Extensions
    {
        #region Description(获取布尔值描述)

        /// <summary>
        /// 获取布尔值描述
        /// </summary>
        /// <param name="value">值</param>
        public static string Description(this bool value) => value ? "是" : "否";

        /// <summary>
        /// 获取布尔值描述
        /// </summary>
        /// <param name="value">值</param>
        public static string Description(this bool? value) => value == null ? "" : Description(value.Value);

        #endregion Description(获取布尔值描述)

        #region FormatInvariant(格式化字符串，不依赖区域性)

        /// <summary>
        /// 格式化字符串，不依赖区域性
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public static string FormatInvariant(this string format, params object[] args) => string.Format(CultureInfo.InvariantCulture, format, args);

        #endregion FormatInvariant(格式化字符串，不依赖区域性)

        #region FormatCurrent(格式化字符串，依赖当前区域性)

        /// <summary>
        /// 格式化字符串，依赖当前区域性
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public static string FormatCurrent(this string format, params object[] args) =>
            string.Format(CultureInfo.CurrentCulture, format, args);

        #endregion FormatCurrent(格式化字符串，依赖当前区域性)

        #region FormatCurrentUI(格式化字符串，依赖当前UI区域性)

        /// <summary>
        /// 格式化字符串，依赖当前UI区域性
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        // ReSharper disable once InconsistentNaming
        public static string FormatCurrentUI(this string format, params object[] args) =>
            string.Format(CultureInfo.CurrentUICulture, format, args);

        #endregion FormatCurrentUI(格式化字符串，依赖当前UI区域性)

        #region FormatMessage(格式化异常消息)

        /// <summary>
        /// 格式化异常消息
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
        public static string FormatMessage(this Exception e, bool isHideStackTrace = false)
        {
            return FormatMessage(e, string.Empty, isHideStackTrace);
        }

        /// <summary>
        /// 格式化异常消息
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="message">消息标题</param>
        /// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
        public static string FormatMessage(this Exception e, string message, bool isHideStackTrace = false)
        {
            var sb = new StringBuilder();
            sb.Append(message);
            var count = 0;
            var appString = string.Empty;
            while (e != null)
            {
                if (count > 0)
                    appString += "  ";
                sb.AppendLine($"{appString}异常消息：{e.Message}");
                sb.AppendLine($"{appString}异常类型：{e.GetType().FullName}");
                sb.AppendLine($"{appString}异常方法：{(e.TargetSite == null ? null : e.TargetSite.Name)}");
                sb.AppendLine($"{appString}异常源：{e.Source}");
                if (!isHideStackTrace && e.StackTrace != null)
                    sb.AppendLine($"{appString}异常堆栈：{e.StackTrace}");
                if (e.InnerException != null)
                {
                    sb.AppendLine($"{appString}内部异常：");
                    count++;
                }
                e = e.InnerException;
            }
            return sb.ToString();
        }

        #endregion FormatMessage(格式化异常消息)
    }
}