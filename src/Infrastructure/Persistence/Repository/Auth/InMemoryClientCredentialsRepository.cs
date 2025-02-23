using de.WebApi.Application.Auth.Token;
using de.WebApi.Application.Common.Persistence;
using de.WebApi.Domain.Auth;

namespace de.WebApi.Infrastructure.Persistence.Repository.Auth;

public class InMemoryClientCredentialsRepository : IClientCredentialsRepository
{
    private readonly List<ClientCredentials> _inMemoryClients;

    public InMemoryClientCredentialsRepository()
    {
        _inMemoryClients = new List<ClientCredentials>();
        _inMemoryClients.Add(new ClientCredentials("myClientId", "m&7yr%KmIsGlH7Y5EDlxuHYs0KkHh!uFRd & qekA@{ ewh~*4IA &= Wcx{ jnTOKv]", true));
        _inMemoryClients.Add(new ClientCredentials("myClientId2", "(N;npZI:Vt[wtf7ko4mT.'^X4iycvka_*:GuNh,{?|F~:>~B!h4|jT4'PH)O/V,", true));
    }

    public Task<ClientCredentials?> GetByClientIdSpecAsync(UserByClientIdSpec specification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ClientCredentials?> GetBySpecAsync(UserByIdAndSecretSpec specification, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(specification.Evaluate(_inMemoryClients).SingleOrDefault());
    }
}