namespace Nigel.Core.ValidationSupport
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:42:50
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 封装所有的对象执行验证。
    /// </summary>
    public class ValidationEvent
    {
        /// <summary>
        /// 验证对象
        /// </summary>
        public object Target;


        /// <summary>
        /// 存储验证错误信息
        /// </summary>
        public IValidationResults Results;


        /// <summary>
        /// 可提供的上下文数据
        /// </summary>
        public object Context;


        public T TargetT<T>()
        {
            if (Target == null)
                return default(T);

            return (T)Target;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="results"></param>
        /// <param name="context"></param>
        public ValidationEvent(object target, IValidationResults results, object context)
        {
            Target = target;
            Results = results;
            Context = context;
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="results"></param>
        public ValidationEvent(object target, IValidationResults results)
        {
            Target = target;
            Results = results;
        }
    }
}
