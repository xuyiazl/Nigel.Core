﻿using Nigel.Extensions;
using System;
using System.Web;

namespace Nigel.Parameters.Parsers
{
    /// <summary>
    /// Url参数解析器
    /// </summary>
    public class UrlParameterParser : ParameterParserBase, IParameterParser
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data">数据</param>
        public override void LoadData(string data)
        {
            if (data.IsEmpty())
            {
                return;
            }

            if (data.Contains("?"))
            {
                data = data.Substring(data.IndexOf("?", StringComparison.Ordinal) + 1);
            }

            var parameters = HttpUtility.ParseQueryString(data);
            foreach (var key in parameters.AllKeys)
            {
                Add(key, parameters.Get(key));
            }
        }
    }
}