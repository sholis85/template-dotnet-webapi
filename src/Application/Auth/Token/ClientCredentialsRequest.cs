using de.WebApi.Domain.Auth;
using de.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Identity;

namespace de.WebApi.Application.Auth.Token;

public class ClientCredentialsRequest : IRequest<TokenDto>
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}

public class ClientCredentialsRequestValidator : CustomValidator<ClientCredentialsRequest>
{
    public ClientCredentialsRequestValidator(IStringLocalizer<ClientCredentialsRequestValidator> T)
    {
        RuleFor(p => p.ClientId)
            .NotEmpty()
            .MaximumLength(75);
        RuleFor(p => p.ClientSecret)
            .NotEmpty()
            .MaximumLength(75);
    }
}

public class ClientCredentialsRequestHandler : IRequestHandler<ClientCredentialsRequest, TokenDto>
{
    private readonly IClientCredentialsRepository _repository;
    private readonly IStringLocalizer<ClientCredentialsRequestHandler> _t;
    private IJwtGenerator _jwtGenerator;

    public ClientCredentialsRequestHandler(IJwtGenerator jwtGenerator, IClientCredentialsRepository repository, IStringLocalizer<ClientCredentialsRequestHandler> t)
        => (_repository, _t, _jwtGenerator) = (repository, t, jwtGenerator);


    public async Task<TokenDto> Handle(ClientCredentialsRequest request, CancellationToken cancellationToken)
    {
        var passwordHasher = new PasswordHasher<string>();

        var user = await _repository.GetByClientIdSpecAsync(new UserByClientIdSpec(request.ClientId), cancellationToken);
        if (user == null)
            throw new NotFoundException(_t["Client not found"]);

        var clientSecret = passwordHasher.VerifyHashedPassword(null, user.ClientSecret, request.ClientSecret);
        if (clientSecret == PasswordVerificationResult.Failed)
            throw new NotFoundException(_t["Client Or Password is incorrect"]);

        _jwtGenerator.AddClaim("sub", user.ClientId);

        // TODO: Add some more claims if necessary

        return new TokenDto() { Token = _jwtGenerator.GetToken(), ExpirationInUnixTime = _jwtGenerator.GetTokenExpirationInUnixTime };
    }
}