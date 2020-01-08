namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:06:26
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogEvent
    {
        public LogLevel Level;
        public string Message;
        public string FinalMessage;
        public Exception Error;
        public string Computer;
        public DateTime CreateTime;
        public string ThreadName;
        public Exception Ex;
        public Type LogType;

        public LogEvent() { }


        public LogEvent(LogLevel level, string message, Exception ex)
        {
            Level = level;
            Message = message;
            Ex = ex;
        }
    }
}
