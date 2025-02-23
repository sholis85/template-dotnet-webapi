using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.WebApi.Infrastructure.Auth.CognitoJwt;
using de.WebApi.Infrastructure.Auth.Handler;
using de.WebApi.Infrastructure.Auth.Requirement;
using de.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.WebApi.Infrastructure.Auth.Policies;

internal static class Startup
{
    internal static AuthorizationOptions AddPolicies(this AuthorizationOptions options, IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(UserGroupSettings)}").Get<UserGroupSettings>();
        options.AddPolicy(AppkPolicies.IsCallcenter, policy =>
            policy.AddRequirements(new CallcenterUserRequeriment(jwtSettings.callcenter)));

        return options;
    }

    internal static IServiceCollection RegisterPolicyRequirements(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<UserGroupSettings>(config.GetSection($"SecuritySettings:{nameof(UserGroupSettings)}"));

        services.AddSingleton<IAuthorizationHandler, CallcenterUserHandler>();

        return services;
    }
}