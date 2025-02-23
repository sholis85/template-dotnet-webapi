using de.WebApi.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Application.Auth.Token;
public class UserByClientIdSpec : Specification<ClientCredentials>, ISingleResultSpecification<ClientCredentials>
{
    public UserByClientIdSpec(string clientId) =>
         Query.Where(b => b.ClientId == clientId);
}
