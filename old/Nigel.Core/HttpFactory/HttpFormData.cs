using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// http formdata 线程安全集合
    /// </summary>
    public class HttpFormData : ConcurrentDictionary<string, object>
    {
        public override string ToString()
        {
            return this.Select(m => m.Key + "=" + m.Value).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n);
        }
    }
}
