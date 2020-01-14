using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature()
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (AuthenticationScheme authenticationScheme in await this.Schemes.GetRequestHandlerSchemesAsync())
            {
                var handlerAsync = await handlers.GetHandlerAsync(context, authenticationScheme.Name) as IAuthenticationRequestHandler;
                bool flag = handlerAsync != null;
                if (flag)
                    flag = await handlerAsync.HandleRequestAsync();
                if (flag)
                    return;
            }
            var authenticateSchemeAsync = await this.Schemes.GetDefaultAuthenticateSchemeAsync();
            if (authenticateSchemeAsync != null)
            {
                //实际的认证业务
                var authenticateResult = await context.AuthenticateAsync(authenticateSchemeAsync.Name);
                if (authenticateResult?.Principal != null)
                    context.User = authenticateResult.Principal;
            }

            await _next.Invoke(context);
        }
    }
}