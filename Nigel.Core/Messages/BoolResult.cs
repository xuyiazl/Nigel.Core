namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 16:45:37
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Nigel.Core.ValidationSupport;

    public class BoolResult<T> : BoolMessageItem<T>
    {
        private IValidationResults _errors;

        /// <summary>
        /// Empty false result.
        /// </summary>
        public static new readonly BoolResult<T> False = new BoolResult<T>(default(T), false, string.Empty, ValidationResults.Empty);


        /// <summary>
        /// Empty True result.
        /// </summary>
        public static new readonly BoolResult<T> True = new BoolResult<T>(default(T), true, string.Empty, ValidationResults.Empty);


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public BoolResult(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message)
        {
            _errors = errors;
        }


        /// <summary>
        /// 异常信息
        /// </summary>
        public IValidationResults Errors
        {
            get { return _errors; }
        }
    }
}
