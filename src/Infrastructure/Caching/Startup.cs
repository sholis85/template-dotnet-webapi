using de.WebApi.Application.Common.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.WebApi.Infrastructure.Caching;
internal static class Startup
{
    internal static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
    {

        services.AddMemoryCache();
        services.AddTransient<ICacheService, LocalCacheService>();

        return services;
    }
}