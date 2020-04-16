using Newtonsoft.Json.Serialization;
using Nigel.Extensions;
using Nigel.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    public interface IRedisSerializer
    {
        RedisValue Serializer<T>(T value);

        T Deserialize<T>(RedisValue value);

        IList<T> Deserialize<T>(RedisValue[] value);
    }

   

}
