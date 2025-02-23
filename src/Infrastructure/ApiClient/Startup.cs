using de.WebApi.Application.ApiClient;
using de.WebApi.Application.ApiClient.TwitterClient;
using de.WebApi.Application.Common.Caching;
using de.WebApi.Infrastructure.ApiClient.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.WebApi.Infrastructure.ApiClient;
internal static class Startup
{
    internal static IServiceCollection AddApiClient(this IServiceCollection services, IConfiguration config)
    {
        // Add api clients here as singletons
        services.AddSingleton<ITwitterClient>(new TwitterClient(
                                                config[$"{ApiClientSettings.Root}:Twitter:baseUrl"],
                                                config[$"{ApiClientSettings.Root}:Twitter:ApiKey"],
                                                config[$"{ApiClientSettings.Root}:Twitter:ApiSecret"]));

        return services;
    }
}