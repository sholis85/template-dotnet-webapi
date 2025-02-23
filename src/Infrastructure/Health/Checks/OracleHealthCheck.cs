using de.WebApi.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace de.WebApi.Infrastructure.Health.Checks;

internal static class OracleHealthCheck
{
    internal static IHealthChecksBuilder AddOracleHealthCheck(this IHealthChecksBuilder builder, IConfiguration config)
    {
        return builder.AddOracle(GetOracleConnectionString(config), failureStatus: HealthStatus.Degraded, name: "Oracle");
    }

    private static string GetOracleConnectionString(IConfiguration config)
    {
        var settings = config.GetSection($"{nameof(DatabaseSettings)}").Get<DatabaseSettings>();
        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new InvalidOperationException("Missing values in the settings for database connection.");

        return settings.ConnectionString;
    }
}