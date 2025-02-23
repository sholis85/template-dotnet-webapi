using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using de.WebApi.Application.Common.Exceptions;
using de.WebApi.Application.Common.Persistence;
using de.WebApi.Infrastructure.Persistence.Repository.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace de.WebApi.Infrastructure.Auth.Jwt;

internal static class Startup
{
    internal static AuthenticationBuilder AddJwtAuth(this AuthenticationBuilder authBuilder, IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection($"SecuritySettings:{nameof(JwtSettings)}"));
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();
        if (string.IsNullOrEmpty(jwtSettings.Key))
            throw new InvalidOperationException("No Key defined in JwtSettings config.");

        switch (jwtSettings.ClientProvider)
        {
            case "inMemory":
                services.AddScoped(typeof(IClientCredentialsRepository), typeof(InMemoryClientCredentialsRepository));
                break;
            case "ef":
                services.AddScoped(typeof(IClientCredentialsRepository), typeof(ApplicationDbClientCredentialsRepository));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(jwtSettings.ClientProvider), "JWT Client provider not supported, options are 'inMemory' or 'ef'");
        }

        byte[] key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        return authBuilder
            .AddJwtBearer("Custom", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            throw new UnauthorizedException("Authentication Failed.");
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = _ => throw new UnauthorizedException("You are not authorized to access this resource."),
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}