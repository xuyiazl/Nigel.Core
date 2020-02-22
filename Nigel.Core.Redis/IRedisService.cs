using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    /// <summary>
    /// 定义redis连接服务
    /// Nigel.Core.Redis.IStackExchangeRedis 已经定义了一些操作
    /// </summary>
    public interface IRedisService : IStackExchangeRedis
    {
        //如果有需要扩充需求可以在此处添加,Nigel.Core.Redis.IStackExchangeRedis中已经包含一些必要的操作了
    }
}
