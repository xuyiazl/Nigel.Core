namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:35:14
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 验证接口
    /// <remarks>不存储任何有状态的实例数据</remarks>
    /// </summary>
    public interface IValidatorNonStateful
    {
        /// <summary>
        /// 验证，并返回<see cref="ValidationResults"/>，
        /// 如果<see cref="ValidationResults"/>有记录，则表示验证失败
        /// </summary>
        /// <param name="target">验证对象</param>
        /// <returns>验证结果</returns>
        IValidationResults ValidateTarget(object target);


        /// <summary>
        /// 验证，并增加<see cref="ValidationResults"/>信息，
        /// 如果<see cref="ValidationResults"/>有记录，则表示验证失败
        /// </summary>
        /// <param name="target">验证对象</param>
        /// <param name="validationResults">收集验证失败的信息</param>
        bool Validate(object target, IValidationResults results);


        /// <summary>
        /// 验证提供的验证事件
        /// </summary>
        /// <param name="validationEvent"></param>
        /// <returns></returns>
        bool Validate(ValidationEvent validationEvent);
    }
}
