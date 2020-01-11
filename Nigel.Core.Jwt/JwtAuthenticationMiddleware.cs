using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        public JwtAuthenticationMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (schemes == null)
                throw new ArgumentNullException(nameof(schemes));
            this._next = next;
            this.Schemes = schemes;
        }

        public IAuthenticationSchemeProvider Schemes { get; set; }

        public async Task Invoke(HttpContext context)
        {
            context.Features.Set<IAuthenticationFeature>((IAuthenticationFeature)new AuthenticationFeature()
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });
            IAuthenticationHandlerProvider handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (AuthenticationScheme authenticationScheme in await this.Schemes.GetRequestHandlerSchemesAsync())
            {
                IAuthenticationRequestHandler handlerAsync = await handlers.GetHandlerAsync(context, authenticationScheme.Name) as IAuthenticationRequestHandler;
                bool flag = handlerAsync != null;
                if (flag)
                    flag = await handlerAsync.HandleRequestAsync();
                if (flag)
                    return;
            }
            AuthenticationScheme authenticateSchemeAsync = await this.Schemes.GetDefaultAuthenticateSchemeAsync();
            if (authenticateSchemeAsync != null)
            {
                AuthenticateResult authenticateResult = await context.AuthenticateAsync(authenticateSchemeAsync.Name);  //实际的认证业务
                if (authenticateResult?.Principal != null)
                    context.User = authenticateResult.Principal;

                
            }

            await _next.Invoke(context);
        }

    }
}