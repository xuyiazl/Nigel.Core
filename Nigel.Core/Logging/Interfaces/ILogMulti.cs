namespace Nigel.Core.Logging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:         
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/23 20:16:56
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ILogMulti : ILog
    {
        ILog this[string loggerName] { get; }

        ILog this[int index] { get; }

        void Append(ILog logger);

        bool ContainsKey(string key);

        void Replace(ILog logger);

        void Clear();

        int Count { get; }
    }
}
