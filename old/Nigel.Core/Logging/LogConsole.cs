namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:27:25
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogConsole : LogBase, ILog
    {
        public LogConsole()
            : base(typeof(LogConsole).FullName)
        {
        }

        public LogConsole(string name)
            : base(name)
        {
        }

        public override void Log(LogEvent logEvent)
        {
            if (!string.IsNullOrEmpty(logEvent.FinalMessage))
                Console.WriteLine(logEvent.FinalMessage);
            else
            {
                string message = BuildMessage(logEvent);
                Console.WriteLine(message);
            }
        }
    }
}
