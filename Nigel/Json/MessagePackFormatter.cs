using MessagePack;
using MessagePack.Formatters;
using Nigel.Helpers;
using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Nigel.Timing;
using MessagePack.Resolvers;

namespace Nigel.Json
{
    public static class MessagePackSerializerFormatterResolver
    {
        public static MessagePackSerializerOptions DateTimeOptions
        {
            get
            {
                var formatter = CompositeResolver.Create(
                              new[] { new DurableDateTimeFormatter() },
                              new[] { ContractlessStandardResolver.Instance });

                return ContractlessStandardResolver.Options.WithResolver(formatter);
            }

        }

        public static MessagePackSerializerOptions UnixDateTimeOptions
        {
            get
            {
                var formatter = CompositeResolver.Create(
                              new[] { new DurableUnixDateTimeFormatter() },
                              new[] { ContractlessStandardResolver.Instance });

                return ContractlessStandardResolver.Options.WithResolver(formatter);
            }

        }

    }

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
                return reader.ReadDateTime().ToUniversalTime();
            }
        }

        public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
        {
            writer.Write(value.ToUniversalTime());
        }
    }

    public class DurableUnixDateTimeFormatter : IMessagePackFormatter<DateTime>
    {
        public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.NextMessagePackType == MessagePackType.Integer)
            {
                var d = reader.ReadInt64();

                return d.ToDateTime();
            }
            else
            {
                return reader.ReadDateTime();
            }
        }

        public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
        {
            writer.Write(value.ToTimeStamp());
        }
    }

}
