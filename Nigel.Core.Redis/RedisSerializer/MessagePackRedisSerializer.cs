using MessagePack;
using Nigel.Extensions;
using Nigel.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.Redis
{
    public class MessagePackRedisSerializer : IRedisSerializer
    {
        public virtual RedisValue Serializer<T>(T value)
        {
            if (value == null) return RedisValue.Null;

            return MessagePackSerializer.Serialize(value);
        }

        public virtual T Deserialize<T>(RedisValue value)
        {
            if (value == RedisValue.Null) return default;

            return MessagePackSerializer.Deserialize<T>(value);
        }

        public virtual IList<T> Deserialize<T>(RedisValue[] value)
        {
            if (value == null) return default;

            IList<T> list = new List<T>();
            foreach (var v in value)
            {
                if (v == RedisValue.Null)
                    list.Add(default);
                else
                    list.Add(MessagePackSerializer.Deserialize<T>(v));
            }

            return list;
        }
    }
}
