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
using System.Security.Claims;
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
                filterContext.Result = new Result(StateCode.Fail, "401", "unauthorized,token does not exist.");
                return;
            }

            var _token = token.SafeString().Substring("Bearer ".Length);

            string _secret = Web.GetService<IOptions<JwtOptions>>().Value.Secret;

            if (!_token.VerifyToken(_secret, out var ex))
            {
                filterContext.Result = new Result(StateCode.Fail, "401", $"verify fail,{ex.Message}");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 是否跳过验证
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
    }
}
