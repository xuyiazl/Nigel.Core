using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Controllers;
using Nigel.Core.Filters;
using Nigel.Core.Razors;
using Nigel.Drawing;

namespace Nigel.WebTests.Controllers
{
    public class testController : ApiControllerBase
    {
        IRazorHtmlGenerator _razorHtmlGenerator;

        public testController(ILogger<testController> logger, IRazorHtmlGenerator razorHtmlGenerator)
            : base(logger)
        {
            _razorHtmlGenerator = razorHtmlGenerator;
        }

        [Route("/api/init")]
        [HttpGet]
        public async Task<IActionResult> Init()
        {
            await _razorHtmlGenerator.Generate();

            return new JsonResult("");
        }

        [Route("/api/error")]
        [HttpGet]
        public IActionResult Error()
        {
            throw new Exception("这里是API异常");
        }

        [Route("/api/success")]
        [HttpGet]
        public async Task<IActionResult> State()
        {
            return Success("000");
        }

        [Route("/api/fail")]
        [HttpGet]
        public async Task<IActionResult> Fail()
        {
            return Fail("000", "测试错误");
        }
    }
}