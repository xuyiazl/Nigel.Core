using Nigel.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Core.HttpFactory
{
    public static class HttpContent<TModel>
    {
        public static HttpContent Create(TModel model, Encoding encoding = null, HttpMediaType mediaType = HttpMediaType.Json)
        {
            switch (mediaType)
            {
                case HttpMediaType.Json:
                    return new StringContent(model.ToJson(), encoding ?? Encoding.UTF8, "application/json");
                case HttpMediaType.MessagePack:
                    return new ByteArrayContent(model.ToMsgPackBytes());
                default:
                    return new StringContent(model.ToJson(), encoding ?? Encoding.UTF8, "application/json");
            }
        }
    }

    public static class HttpContentExtensions
    {
        public static async Task<TModel> ReadAsAsync<TModel>(this HttpContent httpContent, HttpMediaType mediaType = HttpMediaType.Json)
        {
            switch (mediaType)
            {
                case HttpMediaType.MessagePack:
                    {
                        var res = await httpContent.ReadAsByteArrayAsync();

                        return res.ToMsgPackObject<TModel>();
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
