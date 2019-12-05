namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:08:18
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogFormatter
    {

        public static string Format(string formatter, LogEvent logEvent)
        {
            if (string.IsNullOrEmpty(formatter))
                return Format(logEvent);

            return Format(logEvent);
        }

        public static string Format(LogEvent logEvent)
        {
            string message = logEvent.Message == null ? string.Empty : logEvent.Message.ToString();
            string msg = message.ToString();

            // <time>:<thread>:<level>:<loggername>:<message>
            string line = logEvent.CreateTime.ToString();

            if (!string.IsNullOrEmpty(logEvent.ThreadName)) line += ":" + logEvent.ThreadName;
            line += " : " + logEvent.Level.ToString();
            line += " : " + msg;
            return line;
        }

        public static string ConvertToString(object[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();
            foreach (object arg in args)
            {
                if (arg != null)
                    buffer.AppendFormat("{0},", arg.ToString());
            }
            return buffer.ToString();
        }
    }
}
