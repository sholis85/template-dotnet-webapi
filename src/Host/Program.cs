using de.WebApi.Application;
using de.WebApi.Host.Configurations;
using de.WebApi.Host.Controllers;
using de.WebApi.Infrastructure;
using de.WebApi.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Compact;

[assembly: ApiConventionType(typeof(AppkApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Service Starting...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
            .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    // TODO: Get this from configuration
    // Comment if database EF is not need or you don't need to run migrations
    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Service Shutting down...");
    Log.CloseAndFlush();
}
