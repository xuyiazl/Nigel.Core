namespace Nigel.Core.Messages
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/13 21:30:35
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 错误信息存储类
    /// <remarks>1.简单的字符串存储 2.键/值 存储</remarks>
    /// </summary>
    public class Errors : Messages, IErrors
    {
        [Obsolete("Use MessageList")]
        public IList<string> ErrorList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }

        [Obsolete("Use MessageMap")]
        public IDictionary<string, string> ErrorMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }
    }
}
