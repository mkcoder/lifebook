using System;
using Castle.Windsor;
using Microsoft.AspNetCore.Hosting;

namespace lifebook.core.cqrses.Extensions
{
    public static class AddCQRSSupportExtension
    {
        public static IWebHostBuilder AddCQRS_ES(this IWebHostBuilder builder)
        {
            return builder;
        }
    }
}
