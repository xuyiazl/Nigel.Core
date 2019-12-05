namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 16:43:55
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolMessageItem : BoolMessage
    {
        /// <summary>
        /// Item associated with boolean message.
        /// </summary>
        private object _item;


        /// <summary>
        /// True message.
        /// </summary>
        public static new readonly BoolMessageItem True = new BoolMessageItem(null, true, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static new readonly BoolMessageItem False = new BoolMessageItem(null, false, string.Empty);


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public BoolMessageItem(object item, bool success, string message)
            : base(success, message)
        {
            _item = item;
        }


        /// <summary>
        /// Return readonly item.
        /// </summary>
        public object Item
        {
            get { return _item; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolMessageItem<T> : BoolMessageItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public BoolMessageItem(T item, bool success, string message)
            : base(item, success, message)
        {
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
