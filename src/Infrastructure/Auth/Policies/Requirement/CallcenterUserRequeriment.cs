using Microsoft.AspNetCore.Authorization;

namespace de.WebApi.Infrastructure.Auth.Requirement;

public class CallcenterUserRequeriment : IAuthorizationRequirement
{
    public string Callcenter { get; }

    public CallcenterUserRequeriment(string callcenter)
    {
        if (string.IsNullOrWhiteSpace(callcenter))
        {
            throw new ArgumentException($"'{nameof(callcenter)}' cannot be null or whitespace.", nameof(callcenter));
        }

        Callcenter = callcenter;
    }

    public bool IsCallcenter(IEnumerable<string> userGroups)
    {
        return userGroups.Any(t => t.Equals(Callcenter));
    }
}