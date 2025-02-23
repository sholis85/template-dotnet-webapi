using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.WebApi.Domain.Common.Contracts;

namespace de.WebApi.Domain.Auth;
public class ClientCredentials : BaseEntity, IAggregateRoot
{
    public string ClientId { get; private set; }
    public string ClientSecret { get; private set; }
    public bool Enabled { get; private set; }

    public ClientCredentials(string clientId, string clientSecret, bool enabled)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        Enabled = enabled;
    }

    public ClientCredentials Update(string clientSecret)
    {
        if (clientSecret is not null && clientSecret?.Equals(ClientSecret) is not true) ClientSecret = clientSecret;
        return this;
    }

    public ClientCredentials Disable()
    {
        if (Enabled) Enabled = false;
        return this;
    }

    public ClientCredentials Enable()
    {
        if (!Enabled) Enabled = true;
        return this;
    }
}