using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using de.WebApi.Application.Common.Interfaces;
using de.WebApi.Infrastructure.Auth.CognitoJwt;
using de.WebApi.Infrastructure.Auth.Jwt;
using de.WebApi.Infrastructure.Auth.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace de.WebApi.Infrastructure.Auth;

internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddCurrentUser();
        var authBuilder = services.AddAuthentication(authentication =>
        {
            authentication.DefaultAuthenticateScheme = "MultiAuthSchemes";
            authentication.DefaultChallengeScheme = "MultiAuthSchemes";
        })
           .AddJwtAuth(services, config)
         //uncomment if you need to use Cognito
         //}).AddCognitoJwtAuth(services, config);
         .AddPolicyScheme("MultiAuthSchemes", JwtBearerDefaults.AuthenticationScheme, options =>
          {
              options.ForwardDefaultSelector = context =>
              {
                  string authorization = context.Request.Headers[HeaderNames.Authorization];
                  if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                  {
                      var token = authorization.Substring("Bearer ".Length).Trim();
                      var jwtHandler = new JwtSecurityTokenHandler();
                      if (jwtHandler.CanReadToken(token))
                          return "Custom";
                  }
                  return "Custom";
              };
          });
        services.AddAuthorization(options =>
        {
            //uncomment if you need to use Cognito
            //var onlyCognitoPolicyBuilder = new AuthorizationPolicyBuilder("Cognito");
            //options.AddPolicy("CognitoSchema", onlyCognitoPolicyBuilder
            //    .RequireAuthenticatedUser()
            //    .Build());

            var onlyJWTPolicyBuilder = new AuthorizationPolicyBuilder("Custom");
            options.AddPolicy("ClientCrentialsSchema", onlyJWTPolicyBuilder
                .RequireAuthenticatedUser()
                .Build());

            // options.AddPolicies(services, config);
        });

        //services.RegisterPolicyRequirements(config);


        return services;
    }

    internal static IApplicationBuilder UseAuth(this IApplicationBuilder app, IConfiguration config)
    {
        // If not already enabled, you will need to enable ASP.NET Core authentication
        app.UseAuthentication();
        app.UseCurrentUser();
        app.UseAuthorization();
        return app;
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
}