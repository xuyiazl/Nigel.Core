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
    [JwtAuthorize]
    public class TokenController : ApiControllerBase
    {
        JwtOptions _jwtOptions;
        public TokenController(ILogger<TokenController> logger, JwtOptions jwtOptions)
            : base(logger)
        {
            _jwtOptions = jwtOptions;
        }

        [Route("Create")]
        [HttpGet]
        [JwtAllowAnonymous]
        public IActionResult Create()
        {
            var token = new JwtBuilder()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .WithSecret(_jwtOptions.Secret)
               .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
               .AddClaim("claim2", "claim2-value")
               .Build();
            return Success("000001", token);
        }

        [Route("Verify")]
        [HttpGet]
        public IActionResult Verify()
        {
            var users = RouteData.Values.GetValueOrDefault("identity");

            return Success("000002", message: "验证成功");
        }
    }
}