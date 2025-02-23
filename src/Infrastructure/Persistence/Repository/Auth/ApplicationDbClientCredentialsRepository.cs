using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using de.WebApi.Application.Auth.Token;
using de.WebApi.Application.Common.Persistence;
using de.WebApi.Application.Common.Persistence;
using de.WebApi.Domain.Auth;
using de.WebApi.Domain.Common.Contracts;
using de.WebApi.Infrastructure.Persistence.Context;
using Mapster;

namespace de.WebApi.Infrastructure.Persistence.Repository.Auth;

public class ApplicationDbClientCredentialsRepository : IClientCredentialsRepository
{
    private readonly IReadRepository<ClientCredentials> _readRepository;

    public ApplicationDbClientCredentialsRepository(IReadRepository<ClientCredentials> readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<ClientCredentials?> GetBySpecAsync(UserByIdAndSecretSpec specification, CancellationToken cancellationToken = default)
    {
        return await _readRepository.FirstOrDefaultAsync(specification, cancellationToken);
    }

    public async Task<ClientCredentials?> GetByClientIdSpecAsync(UserByClientIdSpec specification, CancellationToken cancellationToken = default)
    {
        return await _readRepository.FirstOrDefaultAsync(specification, cancellationToken);
    }
}