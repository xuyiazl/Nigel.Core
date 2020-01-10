using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Controllers;
using Nigel.Extensions;

namespace Nigel.ApiTests.Controllers
{
    public class TestController : ApiControllerBase
    {
        public TestController(ILogger<TestController> logger)
          : base(logger)
        {

        }

        [Route("create")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return Success("000", null);
        }

        [Route("error")]
        [HttpGet]
        public IActionResult Error()
        {
            throw new Exception("这里是API异常");
        }

        [Route("success")]
        [HttpGet]
        public IActionResult Success()
        {
            return Success("000", new ReturnModel());
        }

        [Route("fail")]
        [HttpGet]
        public IActionResult Fail()
        {
            return Fail("000", "测试错误");
        }
    }
}