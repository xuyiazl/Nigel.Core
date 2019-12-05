namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:45:17
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 验证接口
    /// <remarks>存储验证结果 和 验证对象</remarks>
    /// </summary>
    public interface IValidatorStateful
    {
        /// <summary>
        /// 验证对象
        /// </summary>
        object Target { get; set; }


        /// <summary>
        /// 验证失败的消息
        /// </summary>
        string Message { get; }


        /// <summary>
        /// 是否验证失败
        /// </summary>
        /// <returns></returns>
        bool IsValid { get; }


        /// <summary>
        /// 验证结果
        /// </summary>
        IValidationResults Results { get; }


        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        IValidationResults Validate();


        /// <summary>
        /// 初始化信息验证
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        IValidationResults Validate(IValidationResults results);


        /// <summary>
        /// 清除结果
        /// </summary>
        void Clear();
    }
}
