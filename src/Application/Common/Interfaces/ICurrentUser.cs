using System.Security.Claims;

namespace de.WebApi.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    string GetUserId();

    string? GetFullName();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}