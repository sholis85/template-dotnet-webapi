using de.WebApi.Infrastructure.ApiClient;
using de.WebApi.Infrastructure.Auth.CognitoJwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace de.WebApi.Infrastructure.Health.Checks;

internal static class TcpsHealthCheck
{
    internal static IHealthChecksBuilder AddTcpsHealthCheck(this IHealthChecksBuilder builder, IConfiguration config, List<TcpUrl> urls)
    {
        foreach (var uri in urls)
        {
            builder = builder.AddDnsResolveHealthCheck(
                setup => setup.ResolveHost(uri.Url.Replace("https://", string.Empty).Replace("/", string.Empty)),
                name: $"{uri.Name}_dns",
                failureStatus: HealthStatus.Degraded);
            builder = builder.AddTcpHealthCheck(
                options => options.AddHost(uri.Url.Replace("https://", string.Empty.Replace("/", string.Empty)), uri.Port),
                name: uri.Name,
                failureStatus: HealthStatus.Degraded,
                timeout: TimeSpan.FromSeconds(5));
        }

        return builder;
    }
}