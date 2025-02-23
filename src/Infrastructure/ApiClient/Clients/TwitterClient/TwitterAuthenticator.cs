using Microsoft.Extensions.Azure;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Serialization;

namespace de.WebApi.Infrastructure.ApiClient.Clients;

public class TwitterAuthenticator : AuthenticatorBase
{
    private readonly string _baseUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly Func<HttpMessageHandler, HttpMessageHandler> _httpMessageHandler;

    public TwitterAuthenticator(string baseUrl, string clientId, string clientSecret, Func<HttpMessageHandler, HttpMessageHandler> httpMessageHandler)
        : base(string.Empty)
    {
        _baseUrl = baseUrl;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _httpMessageHandler = httpMessageHandler;
    }

    protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        string token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
        return new HeaderParameter(KnownHeaders.Authorization, token);
    }

    private async Task<string> GetToken()
    {
        var options = new RestClientOptions(_baseUrl)
        {
            ConfigureMessageHandler = _httpMessageHandler,
            ThrowOnAnyError = true,
            Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)
        };

        var client = new RestClient(options);

        var request = new RestRequest("oauth2/token")
            .AddParameter("grant_type", "client_credentials");
        var response = await client.PostAsync<TokenResponse>(request);
        return $"{response!.TokenType} {response!.AccessToken}";
    }

    private record TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }
}
