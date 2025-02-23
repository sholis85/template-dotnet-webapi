using System.Security.Claims;

namespace de.WebApi.Shared.Authorization;
public interface IJwtGenerator
{
    long GetTokenExpirationInUnixTime { get; }

    IJwtGenerator AddClaim(string claimName, string value);
    string GetToken();
}