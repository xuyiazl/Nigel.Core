using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.MessagePack
{
    public class MediaTypeAttribute : ProducesAttribute
    {
        public MediaTypeAttribute() : base("application/x-msgpack", "application/json")
        {

        }

        public MediaTypeAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}
