using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Nigel.Core.Jwt.Algorithms;
using Nigel.Core.Jwt.Builder;
using Nigel.Core.Jwt.Serializers;
using Nigel.Extensions;
using Nigel.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Nigel.Core.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JwtAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!VerifyAttribute(filterContext.ActionDescriptor))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var result = filterContext.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var token);
            if (!result || string.IsNullOrWhiteSpace(token.SafeString()))
            {
                filterContext.Result = new Result(StateCode.Fail, "401", "未授权，token不存在");
                return;
            }

            var _token = token.SafeString().Substring("Bearer ".Length);

            string _secret = Web.GetService<IOptions<JwtOptions>>().Value.Secret;

            if (!VerifyToken(_token, _secret, out var ex))
            {
                filterContext.Result = new Result(StateCode.Fail, "401", $"验证失败，{ex.Message}");
                return;
            }

            try
            {
                var user = new JwtBuilder()
                    .WithSecret(_secret)
                    .MustVerifySignature()
                    .Decode<Dictionary<string, object>>(_token);

                user.TryAdd("token", _token);

                filterContext.RouteData.Values.TryAdd("identity", user);

                base.OnActionExecuting(filterContext);
            }
            catch (TokenExpiredException)
            {
                filterContext.Result = new Result(StateCode.Fail, "401", "验证失败，token过期");
                return;
            }
            catch (SignatureVerificationException)
            {
                filterContext.Result = new Result(StateCode.Fail, "401", "验证失败，token无效");
                return;
            }
        }

        /// <summary>
        /// 验证是否跳过验证
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        private bool VerifyAttribute(ActionDescriptor actionDescriptor)
        {
            var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
            var htmlAttribute = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<JwtAllowAnonymousAttribute>() ??
                              controllerActionDescriptor.MethodInfo.GetCustomAttribute<JwtAllowAnonymousAttribute>();
            return htmlAttribute == null;
        }

        /// <summary>
        /// 验证token完整性和时效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool VerifyToken(string token, string secret, out Exception ex)
        {
            var urlEncoder = new JwtBase64UrlEncoder();
            var jsonNetSerializer = new JsonNetSerializer();
            var utcDateTimeProvider = new UtcDateTimeProvider();

            var jwt = new JwtParts(token);

            var payloadJson = urlEncoder.Decode(jwt.Payload).ToString(Encoding.UTF8);

            var crypto = urlEncoder.Decode(jwt.Signature);
            var decodedCrypto = crypto.ToBase64String();

            var alg = new HMACSHA256Algorithm();
            var bytesToSign = String.Concat(jwt.Header, ".", jwt.Payload).ToBytes(Encoding.UTF8);
            var signatureData = alg.Sign(secret.ToBytes(Encoding.UTF8), bytesToSign);
            var decodedSignature = signatureData.ToBase64String();

            var jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);

            return jwtValidator.TryValidate(payloadJson, decodedCrypto, decodedSignature, out ex);
        }
    }
}
