using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using de.WebApi.Application.Common.Interfaces;
using de.WebApi.Shared.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace de.WebApi.Infrastructure.Auth.Jwt;
public class JwtGenerator : IJwtGenerator, IScopedService
{
    private readonly JwtHeader _jwtHeader;
    private readonly IList<Claim> _jwtClaims;
    private readonly DateTime _jwtDate;
    private readonly int _jwtTokenExpirationInMinutes;
    private readonly int _jwtRefreshTokenExpirationInDays;
    private readonly string _audience;
    private readonly string _issuer;

    public JwtGenerator(IConfiguration config)
    {
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();
        byte[] key = Encoding.UTF8.GetBytes(jwtSettings.Key);
        var credentials = new SigningCredentials(key: new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256);

        _jwtHeader = new JwtHeader(credentials);
        _jwtDate = DateTime.UtcNow;
        _jwtClaims = new List<Claim>();
        _jwtTokenExpirationInMinutes = jwtSettings.TokenExpirationInMinutes;
        _jwtRefreshTokenExpirationInDays = jwtSettings.RefreshTokenExpirationInDays;
        _audience = jwtSettings.Audience ?? "identityapp";
        _issuer = jwtSettings.Issuer ?? "identityapp";
    }

    public IJwtGenerator AddClaim(string claimName, string value)
    {
        _jwtClaims.Add(new Claim(claimName, value));
        return this;
    }

    public long GetTokenExpirationInUnixTime => new DateTimeOffset(_jwtDate.AddMinutes(_jwtTokenExpirationInMinutes)).ToUnixTimeMilliseconds();

    public string GetToken()
    {
        var jwt = new JwtSecurityToken(
            _jwtHeader,
            new JwtPayload(
                audience: _audience,
                issuer: _issuer,
                notBefore: _jwtDate,
                expires: _jwtDate.AddMinutes(_jwtTokenExpirationInMinutes),
                claims: _jwtClaims));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}