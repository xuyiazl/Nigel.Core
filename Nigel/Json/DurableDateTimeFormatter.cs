using MessagePack;
using MessagePack.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Json
{
    public class DurableDateTimeFormatter : IMessagePackFormatter<DateTime>
    {
        public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.NextMessagePackType == MessagePackType.String)
            {
                var str = reader.ReadString();
                
                return DateTime.Parse(str).ToUniversalTime();
            }
            else
            {
                return reader.ReadDateTime();
            }
        }

        public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
        {
            writer.Write(value);
        }
    }
}
