using Nigel.Extensions;
using Nigel.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.HttpFactory
{
    public static class HttpContentMessage
    {
        public static HttpContent CreateJsonContent<TModel>(TModel model, Encoding encoding = null)
            => Create(model, encoding, HttpMediaType.Json);

        public static HttpContent CreateMessagePackContent<TModel>(TModel model, Encoding encoding = null)
            => Create(model, encoding, HttpMediaType.MessagePack);

        public static HttpContent Create<TModel>(TModel model, Encoding encoding = null, HttpMediaType mediaType = HttpMediaType.Json)
        {
            HttpContent content;

            switch (mediaType)
            {
                case HttpMediaType.MessagePack:
                    content = new ByteArrayContent(model.ToMsgPackBytes());
                    break;
                default:
                    content = new StringContent(model.ToJson(), encoding ?? Encoding.UTF8);
                    break;
            }

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType.Description());

            return content;
        }
    }

    public static class HttpContentExtensions
    {
        public static async Task<TModel> ReadAsJsonAsync<TModel>(this HttpContent httpContent)
        {
            return await httpContent.ReadAsAsync<TModel>(HttpMediaType.Json);
        }

        public static async Task<TModel> ReadAsMessagePackAsync<TModel>(this HttpContent httpContent)
        {
            return await httpContent.ReadAsAsync<TModel>(HttpMediaType.MessagePack);
        }

        public static async Task<TModel> ReadAsAsync<TModel>(this HttpContent httpContent, HttpMediaType mediaType)
        {
            switch (mediaType)
            {
                case HttpMediaType.MessagePack:
                    {
                        var res = await httpContent.ReadAsByteArrayAsync();

                        return res.ToMsgPackObject<TModel>();
                    }
                case HttpMediaType.MessagePackJackson:
                    {
                        var res = await httpContent.ReadAsStringAsync();

                        return res.ToMsgPackBytesFromJson().ToMsgPackObject<TModel>();
                    }
                default:
                    {
                        var res = await httpContent.ReadAsStringAsync();

                        return res.ToObject<TModel>();
                    }
            }
        }
    }
}
