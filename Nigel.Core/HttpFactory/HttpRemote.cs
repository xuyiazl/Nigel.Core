﻿using Nigel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.HttpFactory
{
    /// <summary>
    /// Http远程操作
    /// </summary>
    public static class HttpRemote
    {
        /// <summary>
        /// 获取HttpService
        /// </summary>
        public static IHttpService Service
        {
            get
            {
                var httpService = Web.GetService<IHttpService>();

                if (httpService == null)
                    throw new ArgumentNullException($"请注入{nameof(IHttpService)}服务，services.AddHttpService<HttpService>(clientname, [servser])。");

                return httpService;
            }
        }
    }
}
