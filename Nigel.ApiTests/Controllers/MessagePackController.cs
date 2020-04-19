using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nigel.Core;
using Nigel.Core.MessagePack;

namespace Nigel.ApiTests.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [MediaType]
    public class MessagePackController : ControllerBase
    {
        public User Get()
        {
            return new User { Id = 1, Name = "test", CreateTime = DateTime.Now };
        }


        [HttpPost]
        public User Add([FromBody] User user)
        {
            user.Name = "哈哈";
            user.CreateTime = DateTime.Now;

            return user;
        }
    }
}