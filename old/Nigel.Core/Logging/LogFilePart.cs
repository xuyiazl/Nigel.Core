namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/10/14 14:12:20
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Timers;

    public class LogFilePart : LogBase, ILog, IDisposable
    {
        private string _filepath;
        private string _filepathUnique;
        private StreamWriter _writer;
        private int _iterativeFlushCount = 0;
        private int _iterativeFlushPeriod = 4;
        private object _lockerFlush = new object();
        private int _maxFileSizeInMegs = 10;
        private bool _rollFile = true;
        private string name = "";
        private string format = "%date%";
        private double interval = 5 * 1000;
        private Timer _timer = new Timer();
        private StringBuilder strLog = new StringBuilder();

        public LogFilePart(string name, string filepath)
            : this(name, filepath, true, 5)
        {
        }

        public LogFilePart(string name, string filepath, bool rollFile, int maxSizeInMegs)
            : base(name)
        {
            this.name = name;
            _rollFile = rollFile;
            _maxFileSizeInMegs = maxSizeInMegs;
            filepath = string.IsNullOrEmpty(filepath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log\\") : filepath;
            filepath += format;
            _filepath = LogHelper.BuildLogFileName(filepath, name, DateTime.Now);
            _filepathUnique = _filepath;
            FileInfo info = new FileInfo(_filepathUnique);
            if (!info.Directory.Exists)
                info.Directory.Create();
            _writer = new StreamWriter(_filepathUnique, true);

            _timer.Interval = interval;
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                string logContent = strLog.ToString();
                strLog = null;
                strLog = new StringBuilder();
                if (!string.IsNullOrEmpty(logContent))
                {
                    _writer.Write(logContent);

                    FlushCheck();
                }
            }
            catch
            {

            }
        }

        public string FilePath
        {
            get { return _filepathUnique; }
        }

        public override void Log(LogEvent logEvent)
        {
            string logContent = BuilderContent(logEvent);

            if (strLog == null)
                strLog = new StringBuilder();

            strLog.Append(logContent);
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        public override void ShutDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            try
            {
                if (_writer != null)
                {
                    _timer.Stop();
                    _writer.Flush();
                    _writer.Close();
                    _writer = null;
                }
            }
            catch (Exception)
            {
            }
        }

        ~LogFilePart()
        {
            Dispose();
        }

        private void FlushCheck()
        {
            lock (_lockerFlush)
            {
                if (_iterativeFlushCount % _iterativeFlushPeriod == 0)
                {
                    _writer.Flush();
                    _iterativeFlushCount = 1;
                }
                else
                    _iterativeFlushCount++;

                string searchPath = Path.GetFileNameWithoutExtension(_filepath);
                string directory = Path.GetDirectoryName(_filepath);

                if (!searchPath.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    _filepathUnique = _filepath = LogHelper.BuildLogFileName(directory + "\\" + format, name, DateTime.Now);
                    _writer = new StreamWriter(_filepathUnique, true);
                }

                float size = new FileInfo(_filepathUnique).Length / (1024 * 1024);
                if (_rollFile && size >= _maxFileSizeInMegs)
                {
                    try
                    {
                        string[] files = Directory.GetFiles(directory, searchPath + "*", SearchOption.TopDirectoryOnly);

                        _filepathUnique = string.Format("{0}\\{1}-part{2}{3}", directory, searchPath, files.Length,
                            Path.GetExtension(_filepath));
                        _writer.Flush();
                        _writer.Close();
                        _writer = new StreamWriter(_filepathUnique);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
