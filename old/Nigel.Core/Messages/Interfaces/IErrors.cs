namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:21:13
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 异常消息
    /// </summary>
    public interface IErrors : IMessages
    {
        [Obsolete("Use MessageList")]
        IList<string> ErrorList { get; set; }

        [Obsolete("Use MessageList")]
        IDictionary<string, string> ErrorMap { get; set; }
    }
}
