using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Core.MessagePack
{
    public class MessagePackProducesAttribute : ProducesAttribute
    {
        public MessagePackProducesAttribute() : base("application/x-msgpack")
        {

        }
    }
}
