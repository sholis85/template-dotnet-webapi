using de.WebApi.Domain.Catalog;

namespace de.WebApi.Application.ApiClient.TwitterClient;

public class GetUserRequest : IRequest<TwitterUserDto>
{
    public string User { get; set; }

    public GetUserRequest(string user) => User = user;
}

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, TwitterUserDto>
{
    private readonly ITwitterClient _client;
    private readonly IStringLocalizer _t;

    public GetUserRequestHandler(ITwitterClient twitterClient, IStringLocalizer<GetUserRequestHandler> localizer) => (_client, _t) = (twitterClient, localizer);

    public async Task<TwitterUserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _client.GetUser(request.User);
        return await _client.Adapt<TwitterUser, TwitterUserDto>(user);
    }
}