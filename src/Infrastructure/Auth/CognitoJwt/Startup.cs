using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace de.WebApi.Infrastructure.Auth.CognitoJwt;

internal static class Startup
{
    internal static AuthenticationBuilder AddCognitoJwtAuth(this AuthenticationBuilder authBuilder, IServiceCollection services, IConfiguration config)
    {
        services.Configure<CognitoJwtSettings>(config.GetSection($"SecuritySettings:{nameof(CognitoJwtSettings)}"));
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(CognitoJwtSettings)}").Get<CognitoJwtSettings>();
        if (string.IsNullOrEmpty(jwtSettings.Region) || string.IsNullOrEmpty(jwtSettings.UserPoolId)
            || string.IsNullOrEmpty(jwtSettings.AppClientId))
            throw new InvalidOperationException("Missing values in the settings for cognito JWT.");

        string? awsCognitoRegion = jwtSettings.Region;
        string awsCognitoPoolId = jwtSettings.UserPoolId;
        string awsAuthority = $"https://cognito-idp.{awsCognitoRegion}.amazonaws.com/{awsCognitoPoolId}";
        string awsMetaDataAddress = $"{awsAuthority}/.well-known/openid-configuration";
        string awsCognitoClientId = jwtSettings.AppClientId;
        authBuilder
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = awsAuthority,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidAudience = awsCognitoClientId,
                ValidateAudience = true
            };
            options.MetadataAddress = awsMetaDataAddress;
        });
        return authBuilder;
    }
}