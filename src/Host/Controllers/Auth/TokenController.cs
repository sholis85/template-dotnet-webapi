using de.WebApi.Application.Auth.Token;
using de.WebApi.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace de.WebApi.Host.Controllers.Auth;
public class TokenController : VersionNeutralApiController
{
    [AllowAnonymous]
    [HttpPost("")]
    [OpenApiOperation("Get JWT token for the provided credentials", "")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
    [ProducesDefaultResponseType(typeof(ErrorResult))]
    public Task<TokenDto> GetToken(ClientCredentialsRequest request)
    {
        return Mediator.Send(request);
    }
}
