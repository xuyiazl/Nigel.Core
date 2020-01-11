﻿using System;
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
using Nigel.Helpers;
using Nigel.Extensions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
               .JwtId(Id.GuidGenerator.Create())
               .Account("nigel")
               .NickName("哈哈")
               .VerifiedPhoneNumber("19173100454")
               .ExpirationTime(DateTime.UtcNow.AddMinutes(60))
               .Build();

            return Success("000001", token);
        }

        [Route("Verify")]
        [HttpGet]
        public IActionResult Verify()
        {
            var jwtid = HttpContext.User.Identity.GetValue<Guid>(ClaimName.JwtId);
            var account = HttpContext.User.Identity.GetValue<string>(ClaimName.Account);
            var nickname = HttpContext.User.Identity.GetValue<string>(ClaimName.NickName);
            var phone = HttpContext.User.Identity.GetValue<string>(ClaimName.VerifiedPhoneNumber);

            return Success("000002",
                data: new
                {
                    jwtid,
                    account,
                    nickname,
                    phone
                },
                message: "验证成功");
        }
    }
}