﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure.Middlewares
{
    class ByPassAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private string _currentUserId;
        public ByPassAuthMiddleware(RequestDelegate next) // TODO: Repasar este Middleware y hacer algun otro de ejemplo, como el de duración de una request
        {
            _next = next;
            _currentUserId = null;
        }


        public async Task Invoke(HttpContext context)
        {
            System.Diagnostics.Debug.WriteLine(" >>>> ByPassAuthMiddleware - Start"); 
            var path = context.Request.Path;
            if (path == "/noauth")
            {
                var userid = context.Request.Query["userid"];
                if (!string.IsNullOrEmpty(userid))
                {
                    _currentUserId = userid;
                }
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/string";
                await context.Response.WriteAsync($"User set to {_currentUserId}");
            }

            else if (path == "/noauth/reset")
            {
                _currentUserId = null;
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/string";
                await context.Response.WriteAsync($"User set to none. Token required for protected endpoints.");
            }
            else
            {
                var currentUserId = _currentUserId;

                var authHeader = context.Request.Headers["Authorization"];
                if (authHeader != StringValues.Empty)
                {
                    var header = authHeader.FirstOrDefault();
                    if (!string.IsNullOrEmpty(header) && header.StartsWith("Email ") && header.Length > "Email ".Length)
                    {
                        currentUserId = header.Substring("Email ".Length);
                    }
                }


                if (!string.IsNullOrEmpty(currentUserId))
                {
                    var user = new ClaimsIdentity(new[] {
                    new Claim("emails", currentUserId),
                    new Claim("name", "Test user"),
                    new Claim("nonce", Guid.NewGuid().ToString()),
                    new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "ByPassAuthMiddleware"),
                    new Claim("nonce", Guid.NewGuid().ToString()),
                    new Claim("sub", "732ec2ef-3a76-45f1-b5d3-bcba7b855fa7"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname","User"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname","Microsoft")}
                    , "ByPassAuth");

                    context.User = new ClaimsPrincipal(user);
                }

                await _next.Invoke(context);

                System.Diagnostics.Debug.WriteLine(" >>>> ByPassAuthMiddleware - End");
            }
        }
    }
}
