namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 22:18:54
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Nigel.Core.ValidationSupport;

    /// <summary>
    /// Combines a boolean succes/fail flag with a error/status message.
    /// </summary>
    public class BoolMessage
    {
        /// <summary>
        /// True message.
        /// </summary>
        public static readonly BoolMessage True = new BoolMessage(true, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static readonly BoolMessage False = new BoolMessage(false, string.Empty);


        /// <summary>
        /// Success / failure ?
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Error message for failure, status message for success.
        /// </summary>
        public readonly string Message;


        /// <summary>
        /// Set the readonly fields.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public BoolMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

}
