using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.MessagePack
{
    public class MediaTypeHeaderAttribute : ProducesAttribute
    {
        public MediaTypeHeaderAttribute() : base("application/json", "application/x-msgpack")
        {

        }

        public MediaTypeHeaderAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}
