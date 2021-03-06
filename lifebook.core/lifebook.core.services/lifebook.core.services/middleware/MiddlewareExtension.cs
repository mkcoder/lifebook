﻿using lifebook.core.services.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace lifebook.core.services.middleware
{
    public static class MiddlewareExtension
    {
        public static void AddHealthChecks(this IApplicationBuilder application, IConfiguration configuration)
        {
            application.Map("/consul/health", bld => {
                bld.Run(async ctx =>
                { 
                    ctx.Response.StatusCode = StatusCodes.Status200OK;
                    await ctx.Response.WriteAsync("Ok");
                });
            });
        }

        public static void DeregisterService(this IApplicationBuilder application, IConfiguration configuration)
        {
            application.UseMiddleware<DeregisterServiceMiddleware>(configuration);
        }
    }
}
