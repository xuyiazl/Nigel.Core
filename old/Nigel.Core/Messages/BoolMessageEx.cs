namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 16:44:44
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Tuple to store boolean, string message, and Exception object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolMessageEx : BoolMessage
    {
        private Exception _ex;


        /// <summary>
        /// True message.
        /// </summary>
        public static new readonly BoolMessageEx True = new BoolMessageEx(true, null, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static new readonly BoolMessageEx False = new BoolMessageEx(false, null, string.Empty);


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        public BoolMessageEx(bool success, Exception ex, string message)
            : base(success, message)
        {
            _ex = ex;
        }


        /// <summary>
        /// Return readonly item.
        /// </summary>
        public Exception Ex
        {
            get { return _ex; }
        }
    }
}
