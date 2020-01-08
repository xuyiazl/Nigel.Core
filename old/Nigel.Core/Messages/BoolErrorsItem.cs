namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 16:45:58
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Nigel.Core.ValidationSupport;

    /// <summary>
    /// Class to store success/fail message, error collection and some result object(T).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolErrorsItem : BoolMessageItem
    {
        protected IErrors _errors;


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors</param>
        public BoolErrorsItem(object item, bool success, string message, IErrors errors)
            : base(item, success, message)
        {
            _errors = errors;
        }


        /// <summary>
        /// List of errors from performing some action.
        /// </summary>
        public IErrors Errors
        {
            get { return _errors; }
        }
    }



    /// <summary>
    /// Bool result with error collection and resulting item with typed access.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolErrorsItem<T> : BoolErrorsItem
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public BoolErrorsItem(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message, errors)
        {
            _errors = errors;
        }


        /// <summary>
        /// Return item as correct type.
        /// </summary>
        public new T Item
        {
            get { return (T)base.Item; }
        }
    }

}
