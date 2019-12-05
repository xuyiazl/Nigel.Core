namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:09:18
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;

    public class LogHelper
    {

        public void LogToConsole<T>(LogLevel level, string message, Exception ex, params object[] args)
        {
            LogEvent logevent = BuildLogEvent(typeof(T), level, message, ex, null);
            Console.WriteLine(logevent.FinalMessage);
        }

        public static LogEvent BuildLogEvent(Type logType, LogLevel level, string message, System.Exception ex, params object[] args)
        {
            LogEvent logevent = new LogEvent();
            logevent.Level = level;
            logevent.Message = args == null ? message : string.Format(message, args);
            logevent.Error = ex;
            logevent.Computer = System.Environment.MachineName;
            logevent.CreateTime = DateTime.Now;
            logevent.ThreadName = System.Threading.Thread.CurrentThread.Name;
            logevent.LogType = logType;
            logevent.FinalMessage = LogFormatter.Format(null, logevent);
            return logevent;
        }

        public static LogLevel GetLogLevel(string loglevel)
        {
            LogLevel level = (LogLevel)Enum.Parse(typeof(LogLevel), loglevel, true);
            return level;
        }


        /// <summary>
        /// Build the log file name.
        /// </summary>
        /// <param name="appName">E.g. "StockMarketApplication".</param>
        /// <param name="date">E.g. Date to put in the name.</param>
        /// <param name="env">Environment name. E.g. "DEV", "PROD".</param>
        /// <param name="logFileName">E.g. "%name%-%yyyy%-%MM%-%dd%-%env%-%user%.log".
        /// Name of logfile containing substituions. </param>
        /// <returns></returns>
        public static string BuildLogFileName(string logFileName, string appName, DateTime date)
        {
            // Log file name = <app>-<date>-<env>.log
            // e.g.  StockMarketApp-2009-10-30-PROD.log
            IDictionary<string, string> subs = new Dictionary<string, string>();
            subs["%datetime%"] = date.ToString("yyyy-MM-dd-HH-mm-ss");
            subs["%date%"] = date.ToString("yyyy-MM-dd");
            subs["%yyyy%"] = date.ToString("yyyy");
            subs["%MM%"] = date.ToString("MM");
            subs["%dd%"] = date.ToString("dd");
            subs["%MMM%"] = date.ToString("MMM");
            subs["%hh%"] = date.ToString("hh");
            subs["%HH%"] = date.ToString("HH");
            subs["%mm%"] = date.ToString("mm");
            subs["%ss%"] = date.ToString("ss");
            subs["%name%"] = appName;
            subs["%user%"] = Dns.GetHostName();

            foreach (var pair in subs)
            {
                logFileName = logFileName.Replace(pair.Key, pair.Value);
            }

            if (!logFileName.Contains(".log") && !logFileName.Contains(".txt"))
                logFileName += ".log";

            // Replace any left over % with underscore "_".
            logFileName = logFileName.Replace("%", "_");
            logFileName = logFileName.Replace("--", "-");
            logFileName = logFileName.Replace("__", "_");
            if (logFileName.StartsWith("-")) logFileName = "Log" + logFileName;
            if (logFileName.StartsWith("_")) logFileName = "Log" + logFileName;

            return logFileName;
        }
    }
}
