using de.WebApi.Application.Common.ApiClient;

namespace de.WebApi.Application.ApiClient.TwitterClient;

public interface ITwitterClient : IApiClient
{
    Task<TwitterUser> GetUser(string user);
}