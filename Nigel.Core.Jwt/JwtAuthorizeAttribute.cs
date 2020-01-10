using Microsoft.AspNetCore.Mvc.Filters;
using Nigel.Core.Jwt.Algorithms;
using Nigel.Core.Jwt.Serializers;
using Nigel.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Nigel.Core.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JwtAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var result = filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            if (!result || string.IsNullOrWhiteSpace(token.SafeString()))
                throw new UnauthorizedAccessException("未授权，请传递Header头的Authorization参数");

            var _token = token.SafeString().Substring("Bearer ".Length);

            string Secret = "";

            var urlEncoder = new JwtBase64UrlEncoder();
            var jsonNetSerializer = new JsonNetSerializer();
            var utcDateTimeProvider = new UtcDateTimeProvider();

            var jwt = new JwtParts(_token);

            var payloadJson = urlEncoder.Decode(jwt.Payload).ToString(Encoding.UTF8);

            var crypto = urlEncoder.Decode(jwt.Signature);
            var decodedCrypto = Convert.ToBase64String(crypto);

            var alg = new HMACSHA256Algorithm();
            var bytesToSign = String.Concat(jwt.Header, ".", jwt.Payload).ToBytes(Encoding.UTF8);
            var signatureData = alg.Sign(Secret.ToBytes(Encoding.UTF8), bytesToSign);
            var decodedSignature = signatureData.ToBase64String();

            var jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);
            var isValid = jwtValidator.TryValidate(payloadJson, decodedCrypto, decodedSignature, out var ex);

            if (!isValid)
                throw new UnauthorizedAccessException("验证失败，请确认参数是否正确或是否有权限");

            base.OnActionExecuting(filterContext);
        }
    }
}
