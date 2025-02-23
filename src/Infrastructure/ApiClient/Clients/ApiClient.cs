using de.WebApi.Application.Common.ApiClient;
using de.WebApi.Application.Common.Interfaces;
using HttpTracer;
using HttpTracer.Logger;
using Mapster;
using RestSharp;
using Serilog;

namespace de.WebApi.Infrastructure.ApiClient.Clients;

public abstract class ApiClient : IApiClient
{
    protected readonly RestClient _client;
    protected readonly Serilog.ILogger _logger;
    protected Func<HttpMessageHandler, HttpMessageHandler> _httpMessageHandler = handler => new HttpTracerHandler(handler, new ConsoleLogger(), HttpMessageParts.All);
    protected readonly string _baseUrl;
    protected readonly string _clientId;
    protected readonly string _clientSecret;

    protected ApiClient(string baseUrl, string apiKey, string apiKeySecret)
    {
        _baseUrl = baseUrl;
        _clientId = apiKey;
        _clientSecret = apiKeySecret;
        _logger = Log.ForContext(GetType());
        var options = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = _httpMessageHandler,
            ThrowOnAnyError = true,
            Timeout = new TimeSpan(50000),
            Authenticator = new TokenAuthenticator(Task.Run(() => GetToken()).Result)
        };
        _client = new RestClient(options);
    }

    protected abstract Task<string> GetToken();

    public Task<TDto> Adapt<T, TDto>(T source)
        where T : class
        where TDto : class, IDto
    {
        return Task.Run(() => source.Adapt<TDto>());
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }
}