namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:07:30
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public abstract class LogBase : ILog
    {
        #region Protected Data
        protected ILog _parent;
        protected ReaderWriterLock _readwriteLock = new ReaderWriterLock();
        protected int _lockMilliSecondsForRead = 1000;
        protected int _lockMilliSecondsForWrite = 1000;
        #endregion

        #region Constructors

        public LogBase() { }

        public LogBase(Type type)
        {
            this.Name = type.FullName;
            Settings = new LogSettings();
            Settings.Level = LogLevel.Info;
        }

        public LogBase(string name)
        {
            this.Name = name;
            Settings = new LogSettings();
            Settings.Level = LogLevel.Info;
        }
        #endregion

        #region Public Properties

        public virtual string Name { get; set; }

        public ILog Parent { get; set; }

        public LogSettings Settings { get; set; }

        public virtual LogLevel Level
        {
            get { return Settings.Level; }
            set { Settings.Level = value; }
        }

        public virtual bool IsDebugEnabled { get { return IsEnabled(LogLevel.Debug); } }

        public virtual bool IsInfoEnabled { get { return IsEnabled(LogLevel.Info); } }

        public virtual bool IsWarnEnabled { get { return IsEnabled(LogLevel.Warn); } }

        public virtual bool IsErrorEnabled { get { return IsEnabled(LogLevel.Error); } }

        public virtual bool IsFatalEnabled { get { return IsEnabled(LogLevel.Fatal); } }
        #endregion

        #region Public Methods

        public virtual ILog this[string loggerName]
        {
            get { return this; }
        }

        public virtual ILog this[int logIndex]
        {
            get { return this; }
        }

        public virtual bool IsEnabled(LogLevel level)
        {
            return level >= Settings.Level;
        }

        public abstract void Log(LogEvent logEvent);

        public virtual void Flush()
        {

        }

        public virtual void Warn(string format)
        {
            if (IsEnabled(LogLevel.Warn)) InternalLog(LogLevel.Warn, format, null, null);
        }

        public virtual void Warn(string format, params object[] args)
        {
            if (IsEnabled(LogLevel.Warn)) InternalLog(LogLevel.Warn, format, null, args);
        }

        public virtual void Warn(string format, System.Exception exception)
        {
            if (IsEnabled(LogLevel.Warn)) InternalLog(LogLevel.Warn, format, exception, null);
        }

        public virtual void Warn(string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(LogLevel.Warn)) InternalLog(LogLevel.Warn, format, ex, args);
        }

        public virtual void Error(string format)
        {
            if (IsEnabled(LogLevel.Error)) InternalLog(LogLevel.Error, format, null, null);
        }

        public virtual void Error(string format, params object[] args)
        {
            if (IsEnabled(LogLevel.Error)) InternalLog(LogLevel.Error, format, null, args);
        }

        public virtual void Error(string format, System.Exception exception)
        {
            if (IsEnabled(LogLevel.Error)) InternalLog(LogLevel.Error, format, exception, null);
        }

        public virtual void Error(string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(LogLevel.Error)) InternalLog(LogLevel.Error, format, ex, args);
        }

        public virtual void Debug(string format)
        {
            if (IsEnabled(LogLevel.Debug)) InternalLog(LogLevel.Debug, format, null, null);
        }

        public virtual void Debug(string format, params object[] args)
        {
            if (IsEnabled(LogLevel.Debug)) InternalLog(LogLevel.Debug, format, null, args);
        }

        public virtual void Debug(string format, System.Exception exception)
        {
            if (IsEnabled(LogLevel.Debug)) InternalLog(LogLevel.Debug, format, exception, null);
        }

        public virtual void Debug(string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(LogLevel.Debug)) InternalLog(LogLevel.Debug, format, ex, args);
        }

        public virtual void Fatal(string format)
        {
            if (IsEnabled(LogLevel.Fatal)) InternalLog(LogLevel.Fatal, format, null, null);
        }

        public virtual void Fatal(string format, params object[] args)
        {
            if (IsEnabled(LogLevel.Fatal)) InternalLog(LogLevel.Fatal, format, null, args);
        }

        public virtual void Fatal(string format, System.Exception exception)
        {
            if (IsEnabled(LogLevel.Fatal)) InternalLog(LogLevel.Fatal, format, exception, null);
        }

        public virtual void Fatal(string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(LogLevel.Fatal)) InternalLog(LogLevel.Fatal, format, ex, args);
        }

        public virtual void Info(string format)
        {
            if (IsEnabled(LogLevel.Info)) InternalLog(LogLevel.Info, format, null, null);
        }

        public virtual void Info(string format, params object[] args)
        {
            if (IsEnabled(LogLevel.Info)) InternalLog(LogLevel.Info, format, null, args);
        }

        public virtual void Info(string format, System.Exception exception)
        {
            if (IsEnabled(LogLevel.Info)) InternalLog(LogLevel.Info, format, exception, null);
        }

        public virtual void Info(string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(LogLevel.Info)) InternalLog(LogLevel.Info, format, ex, args);
        }

        public virtual void Message(string format)
        {
            InternalLog(LogLevel.Message, format, null, null);
        }

        public virtual void Message(string format, params object[] args)
        {
            InternalLog(LogLevel.Message, format, null, args);
        }

        public virtual void Message(string format, System.Exception exception)
        {
            InternalLog(LogLevel.Message, format, exception, null);
        }

        public virtual void Message(string format, System.Exception ex, params object[] args)
        {
            InternalLog(LogLevel.Message, format, ex, args);
        }

        public virtual void Log(LogLevel level, string format)
        {
            if (IsEnabled(level)) InternalLog(level, format, null, null);
        }

        public virtual void Log(LogLevel level, string format, params object[] args)
        {
            if (IsEnabled(level)) InternalLog(level, format, null, args);
        }

        public virtual void Log(LogLevel level, string format, System.Exception exception)
        {
            if (IsEnabled(level)) InternalLog(level, format, exception, null);
        }

        public virtual void Log(LogLevel level, string format, System.Exception ex, params object[] args)
        {
            if (IsEnabled(level)) InternalLog(level, format, ex, args);
        }

        public virtual void InternalLog(LogLevel level, string format, System.Exception ex, params object[] args)
        {
            LogEvent logevent = BuildLogEvent(level, format, ex, args);
            Log(logevent);
        }

        public virtual void ShutDown()
        {
            Console.WriteLine("Shutting down logger " + this.Name);
        }
        #endregion

        #region Protected Methods

        public virtual LogEvent BuildLogEvent(LogLevel level, string format, System.Exception ex, params object[] args)
        {
            return LogHelper.BuildLogEvent(this.GetType(), level, format, ex, args);
        }

        protected virtual string BuildMessage(LogEvent logEvent)
        {
            return LogFormatter.Format("", logEvent);
        }

        public virtual string BuilderContent(LogEvent logEvent)
        {
            string finalMessage = logEvent.FinalMessage;
            if (string.IsNullOrEmpty(finalMessage))
                finalMessage = BuildMessage(logEvent);

            StringBuilder strLog = new StringBuilder();

            switch (logEvent.Level)
            {
                case LogLevel.Warn:
                case LogLevel.Fatal:
                case LogLevel.Error:
                    {
                        strLog.AppendLine("Level        : " + logEvent.Level);
                        //strLog.AppendLine("Computer     : " + logEvent.Computer);
                        strLog.AppendLine("CreateTime   : " + logEvent.CreateTime);
                        if (logEvent.ThreadName != null)
                            strLog.AppendLine("ThreadName   : " + logEvent.ThreadName);
                        //strLog.AppendLine("LogType      : " + logEvent.LogType);
                        strLog.AppendLine("Message      : " + logEvent.Message);
                        //strLog.AppendLine("FinalMessage : " + logEvent.FinalMessage);
                        if (logEvent.Error != null)
                        {
                            strLog.AppendLine("Error        : " + logEvent.Error);
                            if (logEvent.Error.TargetSite != null)
                                strLog.AppendLine("Method       : " + logEvent.Error.TargetSite);
                        }
                        strLog.AppendLine("-------------------------------------------------------------------");
                    }
                    break;
                default:
                    strLog.AppendLine(finalMessage);
                    break;
            }
            return strLog.ToString();
        }

        #endregion

        #region Synchronization Helper Methods

        protected void ExecuteRead(Action executor)
        {
            AcquireReaderLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ReleaseReaderLock();
            }
        }

        protected void ExecuteWrite(Action executor)
        {
            AcquireWriterLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ReleaseWriterLock();
            }
        }

        protected void AcquireReaderLock()
        {
            _readwriteLock.AcquireReaderLock(_lockMilliSecondsForRead);
        }

        protected void ReleaseReaderLock()
        {
            _readwriteLock.ReleaseReaderLock();
        }

        protected void AcquireWriterLock()
        {
            _readwriteLock.AcquireWriterLock(_lockMilliSecondsForWrite);
        }

        protected void ReleaseWriterLock()
        {
            _readwriteLock.ReleaseWriterLock();
        }
        #endregion
    }
}
