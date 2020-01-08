namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:13:00
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ILog
    {
        string Name { get; }

        LogLevel Level { get; set; }

        void Log(LogEvent logEvent);

        void Warn(string format);

        void Warn(string format, params object[] args);

        void Warn(string format, Exception exception);

        void Warn(string format, Exception exception, params object[] args);

        void Error(string format);

        void Error(string format, params object[] args);

        void Error(string format, Exception exception);

        void Error(string format, Exception exception, params object[] args);

        void Debug(string format);

        void Debug(string format, params object[] args);

        void Debug(string format, Exception exception);

        void Debug(string format, Exception exception, params object[] args);

        void Fatal(string format);

        void Fatal(string format, params object[] args);

        void Fatal(string format, Exception exception);

        void Fatal(string format, Exception exception, params object[] args);

        void Info(string format);

        void Info(string format, params object[] args);

        void Info(string format, Exception exception);

        void Info(string format, Exception exception, params object[] args);

        void Message(string format);

        void Message(string format, params object[] args);

        void Message(string format, Exception exception);

        void Message(string format, Exception exception, params object[] args);

        void Log(LogLevel level, string format);

        void Log(LogLevel level, string format, params object[] args);

        void Log(LogLevel level, string format, Exception exception);

        void Log(LogLevel level, string format, System.Exception ex, params object[] args);

        bool IsEnabled(LogLevel level);

        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }

        LogEvent BuildLogEvent(LogLevel level, string format, System.Exception ex, params object[] args);

        void Flush();

        void ShutDown();
    }
}
