using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.WebApi.Application.Auth.Token;
using de.WebApi.Domain.Auth;

namespace de.WebApi.Application.Common.Persistence;
public interface IClientCredentialsRepository
{
    Task<ClientCredentials?> GetBySpecAsync(UserByIdAndSecretSpec specification, CancellationToken cancellationToken = default);
    Task<ClientCredentials?> GetByClientIdSpecAsync(UserByClientIdSpec specification, CancellationToken cancellationToken = default);
}