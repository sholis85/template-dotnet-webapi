using de.WebApi.Application.ApiClient.TwitterClient;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace de.WebApi.Host.Controllers.Catalog;

public class TwitterController : VersionNeutralApiController
{

    [HttpGet("{user}")]
    [OpenApiOperation("Get Twitter user information", "")]
    public Task<TwitterUserDto> GetAsync(string user)
    {
        return Mediator.Send(new GetUserRequest(user));
    }
}
