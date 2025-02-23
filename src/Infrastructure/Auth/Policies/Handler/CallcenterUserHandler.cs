using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using de.WebApi.Infrastructure.Auth.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace de.WebApi.Infrastructure.Auth.Handler;
public class CallcenterUserHandler : AuthorizationHandler<CallcenterUserRequeriment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CallcenterUserRequeriment requirement)
    {
        if (requirement.IsCallcenter(context.User.GetGroups() ?? Enumerable.Empty<string>()))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}