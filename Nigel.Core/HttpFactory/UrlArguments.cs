namespace Nigel.Core.HttpFactory
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:46:54
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UrlArguments
    {
        private IDictionary<string, object>
                Args = new SortedDictionary<string, object>();

        private string _host;
        public string ClientName { get; set; }
        public string Url { get; private set; }

        public UrlArguments()
        {
        }

        public UrlArguments(string requestUrl)
        {
            _host = requestUrl;
        }

        public UrlArguments(string clientName, string requestUrl)
        {
            _host = requestUrl;
            ClientName = clientName;
        }

        public UrlArguments(string requestUrl, params KeyValuePair<string, object>[] keyValues)
        {
            _host = requestUrl;
            foreach (var item in keyValues)
            {
                this.Add(item.Key, item.Value);
            }
        }

        public UrlArguments(string clientName, string requestUrl, params KeyValuePair<string, object>[] keyValues)
        {
            _host = requestUrl;
            ClientName = clientName;
            foreach (var item in keyValues)
            {
                this.Add(item.Key, item.Value);
            }
        }

        public static UrlArguments Create() => new UrlArguments();

        public static UrlArguments Create(string requestUrl) => Create(string.Empty, requestUrl);

        public static UrlArguments Create(string clientName, string requestUrl) => new UrlArguments(clientName, requestUrl);

        public static UrlArguments Create(string requestUrl, params KeyValuePair<string, object>[] keyValues) => Create(string.Empty, requestUrl, keyValues);

        public static UrlArguments Create(string clientName, string requestUrl, params KeyValuePair<string, object>[] keyValues) => new UrlArguments(clientName, requestUrl, keyValues);

        public static UrlArguments Create(string clientName, string requestUrl, string method) => new UrlArguments(clientName, $"{requestUrl}/{method}");

        public static UrlArguments Create(string clientName, string host, string area = "api", string controller = null, string method = null)
        {
            if (!string.IsNullOrEmpty(method))
            {
                if (!string.IsNullOrEmpty(area))
                {
                    return new UrlArguments(clientName, $"{host}/{area}/{controller}/{method}");
                }
                else
                {
                    return new UrlArguments(clientName, $"{host}/{controller}/{method}");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(area))
                {
                    return new UrlArguments(clientName, $"{host}/{area}/{controller}");
                }
                else
                {
                    return new UrlArguments(clientName, $"{host}/{controller}");
                }
            }
        }

        public UrlArguments SetClientName(string clientName)
        {
            ClientName = clientName;
            return this;
        }

        public UrlArguments SetHost(string host)
        {
            _host = host;
            return this;
        }

        public UrlArguments Add(string key, object value)
        {
            if (!Args.ContainsKey(key))
            {
                Args.Add(key, value);
            }
            else
            {
                Args[key] = value;
            }

            return this;
        }

        public UrlArguments Add(string key, object value, Func<bool> filter)
        {
            if (!filter())
            {
                return this;
            }

            if (!Args.ContainsKey(key))
            {
                Args.Add(key, value);
            }
            else
            {
                Args[key] = value;
            }

            return this;
        }

        public UrlArguments Remove(string key)
        {
            if (Args != null)
            {
                if (Args.ContainsKey(key))
                {
                    Args.Remove(key);
                }
            }

            return this;
        }

        public UrlArguments Clear()
        {
            Args.Clear();
            _host = string.Empty;
            return this;
        }

        public UrlArguments Complete()
        {
            StringBuilder url = new StringBuilder();
            url.Append(_host);

            if (Args.Count == 0)
            {
                Url = url.ToString();
                return this;
            }
            if (!_host.EndsWith("&"))
            {
                url.Append("?");
            }

            url.Append(Args.Select(m => m.Key + "=" + m.Value).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n));

            Url = url.ToString();
            return this;
        }

        public override string ToString()
        {
            return this.Complete().Url;
        }
    }
}