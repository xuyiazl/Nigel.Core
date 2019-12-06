using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// 运行时间计算工具
    /// </summary>
    public static class CodeTimer
    {
        /// <summary>
        /// 计算运行时间
        /// </summary>
        /// <param name="Explain"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static String Run(String Explain, Action action)
        {
            if (action != null)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                action();

                stopWatch.Stop();

                string runTime = SerializationTime(stopWatch);

                return String.Format("[{0}] - Time Elapsed : {1}", Explain, runTime);
            }
            return String.Empty;
        }

        /// <summary>
        /// 计算运行时间
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static String Run(Action action)
        {
            if (action != null)
            {

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                action();

                stopWatch.Stop();

                return String.Format("Time Elapsed : {0}", SerializationTime(stopWatch));
            }
            return String.Empty;
        }

        private static string SerializationTime(Stopwatch stopWatch)
        {
            return string.Format("{0:N0} ms", stopWatch.ElapsedMilliseconds);
            //return String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            //                    stopWatch.Elapsed.Hours,
            //                    stopWatch.Elapsed.Minutes,
            //                    stopWatch.Elapsed.Seconds,
            //                    stopWatch.Elapsed.Milliseconds);
        }

    }
}
