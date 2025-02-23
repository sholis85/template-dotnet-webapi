using de.WebApi.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace de.WebApi.Infrastructure.Health.Checks;

internal static class MysqlHealthCheck
{
    internal static IHealthChecksBuilder AddMysqlHealthCheck(this IHealthChecksBuilder builder, IConfiguration config)
    {
        return builder.AddMySql(GetMysqlConnectionString(config), failureStatus: HealthStatus.Degraded, name: "Mysql");
    }

    private static string GetMysqlConnectionString(IConfiguration config)
    {
        var settings = config.GetSection($"{nameof(DatabaseSettings)}").Get<DatabaseSettings>();
        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new InvalidOperationException("Missing values in the settings for database connection.");

        return settings.ConnectionString;
    }
}