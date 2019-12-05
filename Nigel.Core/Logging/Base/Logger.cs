namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:15:22
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class Logger
    {
        private static Dictionary<string, ILogMulti> _loggers = new Dictionary<string, ILogMulti>()
        {
            { "default", new LogMulti( "default",new LogConsole("console")) }
        };

        private static ReaderWriterLock _readwriteLock = new ReaderWriterLock();

        private static int _lockMilliSecondsForRead = 1000;
        private static int _lockMilliSecondsForWrite = 1000;

        private Logger()
        { }

        #region Log using level

        public static void Log(LogLevel level, string message)
        {
            Log(level, message, null, null);
        }

        public static void Log(LogLevel level, string message, params object[] args)
        {
            Log(level, message, null, args);
        }

        public static void Log(LogLevel level, string message, Exception exception)
        {
            Log(level, message, exception, null);
        }

        public static void Log(LogLevel level, string message, Exception exception, params object[] args)
        {
            LogEvent logEvent = LogHelper.BuildLogEvent(typeof(Logger), level, message, exception, args);

            Default.Log(logEvent);
        }
        #endregion

        #region Log Warnings

        public static void Warn(string message)
        {
            Log(LogLevel.Warn, message, null, null);
        }

        public static void Warn(string message, params object[] arguments)
        {
            Log(LogLevel.Warn, message, null, arguments);
        }

        public static void Warn(string message, System.Exception exception)
        {
            Log(LogLevel.Warn, message, exception, null);
        }

        public static void Warn(string message, System.Exception exception, params object[] arguments)
        {
            Log(LogLevel.Warn, message, exception, arguments);
        }
        #endregion

        #region Log Errors

        public static void Error(string message)
        {
            Log(LogLevel.Error, message, null, null);
        }

        public static void Error(string message, params object[] arguments)
        {
            Log(LogLevel.Error, message, null, arguments);
        }

        public static void Error(string message, System.Exception exception)
        {
            Log(LogLevel.Error, message, exception, null);
        }

        public static void Error(string message, System.Exception exception, params object[] arguments)
        {
            Log(LogLevel.Error, message, exception, arguments);
        }
        #endregion

        #region Log Debug

        public static void Debug(string message)
        {
            Log(LogLevel.Debug, message, null, null);
        }

        public static void Debug(string message, params object[] arguments)
        {
            Log(LogLevel.Debug, message, null, arguments);
        }

        public static void Debug(string message, System.Exception exception)
        {
            Log(LogLevel.Debug, message, exception, null);
        }

        public static void Debug(string message, System.Exception exception, params object[] arguments)
        {
            Log(LogLevel.Debug, message, exception, arguments);
        }
        #endregion

        #region Log Fatal

        public static void Fatal(string message)
        {
            Log(LogLevel.Fatal, message, null, null);
        }

        public static void Fatal(string message, params object[] arguments)
        {
            Log(LogLevel.Fatal, message, null, arguments);
        }

        public static void Fatal(string message, System.Exception exception)
        {
            Log(LogLevel.Fatal, message, exception, null);
        }

        public static void Fatal(string message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Fatal, message, exception, arguments);
        }
        #endregion

        #region Log Info

        public static void Info(string message)
        {
            Log(LogLevel.Info, message, null, null);
        }

        public static void Info(string message, params object[] arguments)
        {
            Log(LogLevel.Info, message, null, arguments);
        }

        public static void Info(string message, System.Exception exception)
        {
            Log(LogLevel.Info, message, exception, null);
        }

        public static void Info(string message, System.Exception exception, params object[] arguments)
        {
            Log(LogLevel.Info, message, exception, arguments);
        }
        #endregion

        #region Log Message

        public static void Message(string message)
        {
            Log(LogLevel.Message, message, null, null);
        }

        public static void Message(string message, params object[] arguments)
        {
            Log(LogLevel.Message, message, null, arguments);
        }

        public static void Message(string message, System.Exception exception)
        {
            Log(LogLevel.Message, message, exception, null);
        }

        public static void Message(string message, System.Exception exception, params object[] arguments)
        {
            Log(LogLevel.Message, message, exception, arguments);
        }
        #endregion

        #region Non-Logging Methods

        public static void Add(ILogMulti logger)
        {
            ExecuteWrite(() => _loggers[logger.Name] = logger);
        }

        /// <summary>       
        /// 清除logs,保留默认logs -- logConsole
        /// </summary>
        public static void Clear()
        {
            ExecuteWrite(() =>
            {
                _loggers.Clear();
                _loggers["default"] = new LogMulti("default", new LogConsole());
            });
        }

        /// <summary>
        /// 获取默认log
        /// </summary>
        public static ILogMulti Default
        {
            get { return Get("default"); }
        }

        /// <summary>       
        /// Message &lt; Debug &lt; Info &lt; Warn &lt; Error &lt; Fatal
        /// </summary>
        /// <param name="loglevel"></param>
        /// <returns></returns>
        public static LogLevel Parse(string loglevel)
        {
            return (LogLevel)Enum.Parse(typeof(LogLevel), loglevel, true);
        }

        /// <summary>
        /// 获取logs数量
        /// </summary>
        public static int Count
        {
            get
            {
                int count = 0;
                ExecuteRead(() => count = _loggers.Count);
                return count;
            }
        }

        /// <summary>
        /// 获取logs
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogMulti Get(string name)
        {
            ILogMulti logger = null;
            ExecuteRead(() =>
            {
                if (!_loggers.ContainsKey(name))
                    return;

                logger = _loggers[name];
            });

            return logger;
        }

        /// <summary>
        /// 获取logs
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogMulti Get(int index)
        {
            ILogMulti logger = null;
            if (index < 0) return logger;

            ExecuteRead(() =>
            {
                if (index >= _loggers.Count)
                    return;

                logger = _loggers[index.ToString()];
            });

            return logger;
        }

        /// <summary>
        /// 初始化default log
        /// </summary>
        /// <param name="logger"></param>
        public static void Init(ILogMulti logger)
        {
            ExecuteWrite(() =>
            {
                if (_loggers == null) _loggers = new Dictionary<string, ILogMulti>();
                _loggers["default"] = new LogMulti("default", logger);
            });
        }

        /// <summary>
        /// Flushes the buffers.
        /// </summary>
        public static void Flush()
        {
            ExecuteRead(() =>
                {
                    foreach (var logger in _loggers)
                        logger.Value.Flush();
                });
        }

        /// <summary>
        /// Shutdown all
        /// </summary>
        public static void ShutDown()
        {
            Flush();
            ExecuteRead(() =>
            {
                foreach (var logger in _loggers)
                    logger.Value.ShutDown();
            });
        }

        #endregion

        protected static void ExecuteRead(Action executor)
        {
            AcquireReaderLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute write action in Logger." + ex.Message);
            }
            finally
            {
                ReleaseReaderLock();
            }
        }

        protected static void ExecuteWrite(Action executor)
        {
            AcquireWriterLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute write action in Logger." + ex.Message);
            }
            finally
            {
                ReleaseWriterLock();
            }
        }

        protected static void AcquireReaderLock()
        {
            _readwriteLock.AcquireReaderLock(_lockMilliSecondsForRead);
        }

        protected static void ReleaseReaderLock()
        {
            _readwriteLock.ReleaseReaderLock();
        }

        protected static void AcquireWriterLock()
        {
            _readwriteLock.AcquireWriterLock(_lockMilliSecondsForWrite);
        }

        protected static void ReleaseWriterLock()
        {
            _readwriteLock.ReleaseWriterLock();
        }
    }
}
