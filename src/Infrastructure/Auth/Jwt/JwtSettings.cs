namespace de.WebApi.Infrastructure.Auth.Jwt;

public class JwtSettings
{
    public bool Enabled { get; set; }

    public string ClientProvider { get; set; }

    public string? Key { get; set; }

    public int TokenExpirationInMinutes { get; set; }

    public int RefreshTokenExpirationInDays { get; set; }

    public string? Audience { get; set; }

    public string? Issuer { get; set; }
}