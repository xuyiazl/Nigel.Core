﻿using Microsoft.AspNetCore.Authorization;
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

            var user = filterContext.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new Result(StateCode.Fail, "401", "unauthorized.");
                return;
            }

            //jwt为无状态，可以在此处维护一个状态机制

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
