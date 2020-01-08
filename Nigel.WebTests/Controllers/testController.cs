using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nigel.Core.Razors;
using Nigel.Drawing;

namespace Nigel.WebTests.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class testController : ControllerBase
    {
        IRazorHtmlGenerator _razorHtmlGenerator;

        public testController(IRazorHtmlGenerator razorHtmlGenerator)
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

        [Route("/api/verifycode")]
        [HttpGet]
        public async Task<IActionResult> VerifyCode()
        {
            CaptchaBuilder c = new CaptchaBuilder();
            string code = c.GetCode(6, CaptchaType.ChineseChar);
            var codeImage = c.CreateImage(code);
            var codeBuffer = ImageHelper.ToBytes(codeImage);
            return File(codeBuffer, @"image/jpeg");
        }
    }
}