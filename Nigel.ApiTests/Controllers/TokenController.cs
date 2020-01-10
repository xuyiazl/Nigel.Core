using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Controllers;
using Nigel.Core.Jwt;
using Nigel.Core.Jwt.Algorithms;
using Nigel.Core.Jwt.Builder;

namespace Nigel.ApiTests.Controllers
{
    public class TokenController : ApiControllerBase
    {
        JwtSettings _jwtSettings;
        public TokenController(ILogger<TokenController> logger, JwtSettings jwtSettings)
            : base(logger)
        {
            _jwtSettings = jwtSettings;
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_jwtSettings.Secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                .AddClaim("claim1", "0")
                .AddClaim("claim2", "claim2-value")
                .Build();

            return Success("000001", token);
        }

        [Route("Verify")]
        [HttpGet]
        [JwtAuthorize]
        public IActionResult Verify()
        {
            //try
            //{
            //    string token = "";
            //    var json = new JwtBuilder()
            //        .WithSecret(_jwtSettings.Secret)
            //        .MustVerifySignature()
            //        .Decode(token);

            //    return Success("000002", message: "验证成功");
            //}
            //catch (TokenExpiredException)
            //{
            //    return Fail("000004", message: "Token 已过期");
            //}
            //catch (SignatureVerificationException)
            //{
            //    return Fail("000003", message: "Token 无效");
            //}
               return Success("000002", message: "验证成功");
        }
    }
}