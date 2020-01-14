using System;

namespace Nigel.Core.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JwtAllowAnonymousAttribute : Attribute
    {
    }
}