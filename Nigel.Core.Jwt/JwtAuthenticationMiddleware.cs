using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Nigel.Core.Jwt;
using Nigel.Core.Jwt.Algorithms;
using Nigel.Core.Jwt.Serializers;
using Nigel.Extensions;
using Nigel.Helpers;
using Nigel.Json;

namespace Nigel.Core.Jwt
{
    public sealed class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtOptions _options;
        public JwtAuthenticationMiddleware(RequestDelegate next, JwtOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var result = context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var token);
            if (!result || string.IsNullOrWhiteSpace(token.SafeString()))
            {
                //throw new UnauthorizedAccessException("未授权，请传递Header头的Authorization参数");
                //context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var res = new Result(StateCode.Fail, "401", "未授权，token不存在");

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonHelper.ToJson(new
                {
                    res.Code,
                    res.SubCode,
                    res.Message,
                    res.ElapsedTime,
                    res.OperationTime,
                    res.Data
                }));

                return;
            }

            var _token = token.SafeString().Substring("Bearer ".Length);

            string _secret = _options.Secret;

            if (!VerifyToken(_token, _secret, out var ex))
            {
                //throw new UnauthorizedAccessException("验证失败，请确认参数是否正确或是否有权限");
                //context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var res = new Result(StateCode.Fail, "401", $"验证失败，{ex.Message}");

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonHelper.ToJson(new
                {
                    res.Code,
                    res.SubCode,
                    res.Message,
                    res.ElapsedTime,
                    res.OperationTime,
                    res.Data
                }));

                return;
            }
            
            await _next.Invoke(context);
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