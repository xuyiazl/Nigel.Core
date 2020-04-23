using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.MessagePack
{
    public class MessagePackRequestContentTypeAttribute : ProducesAttribute
    {
        public MessagePackRequestContentTypeAttribute() : base("application/json")
        {

        }

        public MessagePackRequestContentTypeAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}
