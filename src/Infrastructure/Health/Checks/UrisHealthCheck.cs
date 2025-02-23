using de.WebApi.Infrastructure.ApiClient;
using de.WebApi.Infrastructure.Auth.CognitoJwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace de.WebApi.Infrastructure.Health.Checks;

internal static class UrisHealthCheck
{
    internal static IHealthChecksBuilder AddUrisHealthCheck(this IHealthChecksBuilder builder, IConfiguration config, List<HealthUrl> urls)
    {
        foreach (var uri in urls)
        {
            builder = builder.AddUrlGroup(new Uri(uri.Url), failureStatus: HealthStatus.Degraded, name: uri.Name);
        }

        return builder;
    }
}