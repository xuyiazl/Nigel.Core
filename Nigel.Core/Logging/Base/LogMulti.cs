namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 20:15:52
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogMulti : LogBase, ILogMulti
    {
        private Dictionary<string, ILog> _loggers;
        private LogLevel _lowestLevel = LogLevel.Debug;


        /// <summary>
        /// Initalize multiple loggers.
        /// </summary>
        /// <param name="loggers"></param>
        public LogMulti(string name, ILog logger)
            : base(typeof(LogMulti).FullName)
        {
            Init(name, new List<ILog>() { logger });
        }


        /// <summary>
        /// Initalize multiple loggers.
        /// </summary>
        /// <param name="loggers"></param>
        public LogMulti(string name, IList<ILog> loggers)
            : base(typeof(LogMulti).FullName)
        {
            Init(name, loggers);
        }


        /// <summary>
        /// Initialize with loggers.
        /// </summary>
        /// <param name="loggers"></param>
        public void Init(string name, IList<ILog> loggers)
        {
            this.Name = name;
            _loggers = new Dictionary<string, ILog>();
            foreach (var logger in loggers)
            {
                _loggers.Add(logger.Name, logger);
            }
            ActivateOptions();
        }


        /// <summary>
        /// Log the event to each of the loggers.
        /// </summary>
        /// <param name="logEvent"></param>
        public override void Log(LogEvent logEvent)
        {
            // Log using the readerlock.
            ExecuteRead(() =>
            {
                foreach (var logger in _loggers)
                {
                    logger.Value.Log(logEvent);
                }
            });
        }


        /// <summary>
        /// Append to the chain of loggers.
        /// </summary>
        /// <param name="logger"></param>
        public void Append(ILog logger)
        {
            ExecuteWrite(() =>
            {
                _loggers.Add(logger.Name, logger);
            });
        }

        public bool ContainsKey(string key)
        {
            bool relust = false;
            ExecuteRead(() =>
            {
                relust = _loggers.ContainsKey(key);
            });
            return relust;
        }

        /// <summary>
        /// Replaces all the existing loggers w/ the supplied logger.
        /// </summary>
        /// <param name="logger"></param>
        public void Replace(ILog logger)
        {
            Clear();
            Append(logger);
        }


        /// <summary>
        /// Get the number of loggers that are part of this loggerMulti.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                ExecuteRead(() => count = _loggers.Count);
                return count;
            }
        }


        /// <summary>
        /// Clear all the exiting loggers and only add the console logger.
        /// </summary>
        public void Clear()
        {
            ExecuteWrite(() =>
            {
                _loggers.Clear();
                _lowestLevel = LogLevel.Message;
                _loggers.Add("console", new LogConsole());
            });
        }


        /// <summary>
        /// Get a logger by it's name.
        /// </summary>
        /// <param name="logger"></param>
        public override ILog this[string loggerName]
        {
            get
            {
                ILog logger = null;
                ExecuteRead(() =>
                {
                    if (!_loggers.ContainsKey(loggerName))
                        return;

                    logger = _loggers[loggerName];
                });
                return logger;
            }
        }


        /// <summary>
        /// Get a logger by it's name.
        /// </summary>
        /// <param name="logger"></param>
        public override ILog this[int logIndex]
        {
            get
            {
                ILog logger = null;
                if (logIndex < 0) return null;

                ExecuteRead(() =>
                {
                    if (logIndex >= _loggers.Count)
                        return;

                    logger = _loggers[logIndex.ToString()];
                });
                return logger;
            }
        }


        /// <summary>
        /// Get the level. ( This is the lowest level of all the loggers. ).
        /// </summary>
        public override LogLevel Level
        {
            get { return _lowestLevel; }
            set
            {
                ExecuteWrite(() =>
                {
                    foreach (var logger in _loggers)
                    {
                        logger.Value.Level = value;
                    }
                    _lowestLevel = value;
                });
            }
        }


        /// <summary>
        /// Whether or not the level specified is enabled.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public override bool IsEnabled(LogLevel level)
        {
            return _lowestLevel <= level;
        }


        /// <summary>
        /// Flushes the buffers.
        /// </summary>
        public override void Flush()
        {
            ExecuteRead(() =>
            {
                foreach (var logger in _loggers)
                {
                    logger.Value.Flush();
                }
            });
        }


        /// <summary>
        /// Shutdown all loggers.
        /// </summary>
        public override void ShutDown()
        {
            ExecuteRead(() =>
            {
                foreach (var logger in _loggers)
                {
                    logger.Value.ShutDown();
                }
            });
        }


        #region Helper Methods
        /// <summary>
        /// Determine the lowest level by getting the lowest level
        /// of all the loggers.
        /// </summary>
        public void ActivateOptions()
        {
            // Get the lowest level from all the loggers.
            ExecuteRead(() =>
            {
                LogLevel level = LogLevel.Fatal;
                foreach (var logger in _loggers)
                {
                    if (logger.Value.Level <= level) level = logger.Value.Level;
                }
                _lowestLevel = level;
            });
        }
        #endregion


    }
}
