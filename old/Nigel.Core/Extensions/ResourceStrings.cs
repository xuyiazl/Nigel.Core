namespace Nigel.Core
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.225
    *           Created by XUYI at 2011/5/9 16:59:43
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Resources;
    using System.Globalization;

    class ResourceStrings
    {
        private static ResourceManager SystemResMgr;
        private static object ResMgrLockObject;

        /// <summary>
        /// To be able to reuse error messages from mscorlib a ResourceManager singleton is instatiated
        /// </summary>
        /// <returns></returns>
        private static ResourceManager InitResourceManager()
        {
            if (SystemResMgr == null)
            {
                lock (typeof(Environment))
                {
                    if (SystemResMgr == null)
                    {
                        // Do not reorder these two field assignments.
                        ResMgrLockObject = new Object();
                        SystemResMgr = new ResourceManager("mscorlib", typeof(String).Assembly);
                    }
                }
            }
            return SystemResMgr;
        }

        internal static String GetString(String key)
        {
            if (SystemResMgr == null)
                InitResourceManager();
            String s;
            lock (ResMgrLockObject)
            {
                s = SystemResMgr.GetString(key, null);
            }
            return s;
        }

        internal static String GetString(String key, params object[] args)
        {
            if (SystemResMgr == null)
                InitResourceManager();
            String format;
            lock (ResMgrLockObject)
            {
                format = SystemResMgr.GetString(key, null);
            }
            if ((args == null) || (args.Length <= 0))
            {
                return format;
            }
            for (int i = 0; i < args.Length; i++)
            {
                string str2 = args[i] as string;
                if ((str2 != null) && (str2.Length > 0x400))
                {
                    args[i] = str2.Substring(0, 0x3fd) + "...";
                }
            }
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
    }

}
