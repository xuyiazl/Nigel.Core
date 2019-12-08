namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 17:15:41
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Reflection;

    public class LogFile : LogBase, ILog, IDisposable
    {
        private string directory;
        private object _lockerFlush = new object();
        private string filerule;
        private string appname;

        public LogFile()
            : this("file", string.Empty)
        { }

        public LogFile(string __directory)
            : this("file", __directory)
        {
        }

        public LogFile(string name, string __directory)
            : base(name)
        {
            appname = "file";
            filerule = "%date%.log";
            directory = __directory;
        }

        public override void Log(LogEvent logEvent)
        {
            lock (_lockerFlush)
            {
                string m_directory = string.Empty;
                if (!string.IsNullOrEmpty(directory))
                    m_directory = directory;
                else
                    m_directory = AppDomain.CurrentDomain.BaseDirectory + "log\\";

                string directoryRule = logEvent.Level.ToString() + "\\" + filerule;

                directoryRule = LogHelper.BuildLogFileName(directoryRule, appname, DateTime.Now);

                FileInfo info = new FileInfo(Path.Combine(m_directory, directoryRule));

                if (!Directory.Exists(info.DirectoryName))
                    Directory.CreateDirectory(info.DirectoryName);

                string logContent = BuilderContent(logEvent);

                System.IO.File.AppendAllText(info.FullName, logContent);
            }
        }

        public void Dispose()
        {

        }
    }
}
