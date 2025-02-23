using de.WebApi.Application.BackgroundJobs;
using de.WebApi.Infrastructure.Persistence;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace de.WebApi.Infrastructure.BackgroundJobs;

internal static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {
        if (bool.TryParse(config["HangfireSettings:Enabled"] ?? "false", out bool isEnabled)) ;
        if (isEnabled)
        {
            services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));

            services.AddHangfireConsoleExtensions();

            var storageSettings = config.GetSection("HangfireSettings:Storage").Get<HangfireStorageSettings>();

            // Important: We can use a differente provider (mysql, oracle, etc) and connectionstring
            if (string.IsNullOrEmpty(storageSettings.StorageProvider)) throw new Exception("Hangfire Storage Provider is not configured.");
            if (string.IsNullOrEmpty(storageSettings.ConnectionString)) throw new Exception("Hangfire Storage Provider ConnectionString is not configured.");
            _logger.Information($"Hangfire: Current Storage Provider : {storageSettings.StorageProvider}");
            _logger.Information("For more Hangfire storage, visit https://www.hangfire.io/extensions.html");

            services.AddSingleton<JobActivator, AppkJobActivator>();

            services.AddHangfire((provider, hangfireConfig) => hangfireConfig
                .UseDatabase(storageSettings.StorageProvider, storageSettings.ConnectionString, config)
                .UseFilter(new AppkJobFilter(provider))
                .UseFilter(new LogJobFilter())
                .UseConsole());
        }

        return services;
    }

    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string dbProvider, string connectionString, IConfiguration config) =>
        dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql =>
                hangfireConfig.UsePostgreSqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<PostgreSqlStorageOptions>()),
            DbProviderKeys.MySql =>
                hangfireConfig.UseStorage(new MySqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<MySqlStorageOptions>())),
            _ => throw new Exception($"Hangfire Storage Provider {dbProvider} is not supported.")
        };

    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        if (bool.TryParse(config["HangfireSettings:Enabled"] ?? "false", out bool isEnabled)) ;
        if (isEnabled)
        {
            var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();

            dashboardOptions.Authorization = new[]
            {
               new HangfireCustomBasicAuthenticationFilter
               {
                    User = config.GetSection("HangfireSettings:Credentials:User").Value,
                    Pass = config.GetSection("HangfireSettings:Credentials:Password").Value
               }
            };

            app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);

        }

        return app;
    }
}