﻿using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Admin.UI.Middlewares
{
    public class AuthenticatedTestRequestMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly string TestAuthorizationHeader = "FakeAuthorization";
        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(TestAuthorizationHeader))
            {
                var token = context.Request.Headers[TestAuthorizationHeader].Single();
                var jwt = new JwtSecurityToken(token);
                var claimsIdentity = new ClaimsIdentity(jwt.Claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.User = claimsPrincipal;
            }

            await _next(context);
        }
    }
}
