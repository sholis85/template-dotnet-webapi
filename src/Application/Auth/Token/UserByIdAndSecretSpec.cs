using de.WebApi.Domain.Auth;

namespace de.WebApi.Application.Auth.Token;

public class UserByIdAndSecretSpec : Specification<ClientCredentials>, ISingleResultSpecification<ClientCredentials>
{
    public UserByIdAndSecretSpec(string clientId, string clientSecret) =>
        Query.Where(b => b.ClientId == clientId && b.ClientSecret == clientSecret);
}