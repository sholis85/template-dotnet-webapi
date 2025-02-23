using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.ApiClient.Clients;
public class TokenAuthenticator : AuthenticatorBase
{
    private readonly string _token;

    public TokenAuthenticator(string token) : base(token)
    {
        _token = token;
    }

    protected async override ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        string token = string.IsNullOrEmpty(Token) ? accessToken : Token;
        return new HeaderParameter(KnownHeaders.Authorization, token);
    }
}