﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Middleware;
internal static class Startup
{
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
        services.AddScoped<ExceptionMiddleware>();

    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();

    internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }

        return services;
    }

    internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }

        return app;
    }

    private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
        config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();
}
