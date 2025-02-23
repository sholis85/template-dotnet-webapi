using de.WebApi.Application.ApiClient;
using de.WebApi.Application.ApiClient.TwitterClient;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.ApiClient.Clients;
public class TwitterClient : ApiClient, ITwitterClient
{
    private record TwitterSingleObject<T>(T Data);

    public TwitterClient(string baseUrl, string apiKey, string apiKeySecret)
        : base(baseUrl, apiKey, apiKeySecret)
    {
    }

    public async Task<TwitterUser> GetUser(string user)
    {
        var response = await _client.GetAsync<TwitterSingleObject<TwitterUser>>(
            "users/by/username/{user}",
            new { user });
        return response!.Data;
    }

    protected async override Task<string> GetToken()
    {
        var options = new RestClientOptions(_baseUrl)
        {
            ConfigureMessageHandler = _httpMessageHandler,
            ThrowOnAnyError = true,
            Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)
        };
        try
        {
            var client = new RestClient(options);
            var request = new RestRequest("oauth2/token")
                .AddParameter("grant_type", "client_credentials");
            var response = await client.PostAsync<TokenResponse>(request);
            return $"{response!.TokenType} {response!.AccessToken}";
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return string.Empty;
        }
    }

    private record TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }
}
