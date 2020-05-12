﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using MessagePack;
using Microsoft.Net.Http.Headers;
using System.Text;
using Nigel.Extensions;

namespace Nigel.Core.MessagePack
{
    public class MessagePackOutputFormatter : OutputFormatter
    {
        private readonly MessagePackFormatterOptions _options;

        public MessagePackOutputFormatter(MessagePackFormatterOptions messagePackFormatterOptions)
        {
            _options = messagePackFormatterOptions ?? throw new ArgumentNullException(nameof(messagePackFormatterOptions));
            foreach (var contentType in messagePackFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.ContentType.SafeString().ToLower() == "application/x-msgpack-jackson")
            {
                var res = MessagePackSerializer.SerializeToJson(context.Object, _options.Options);

                var bytes = Encoding.UTF8.GetBytes(res);

                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
            else
            {
                await MessagePackSerializer.SerializeAsync(context.ObjectType, context.HttpContext.Response.Body, context.Object, _options.Options);
            }
        }
    }
}
