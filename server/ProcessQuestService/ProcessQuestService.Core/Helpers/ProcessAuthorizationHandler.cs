﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProcessQuestService.Core.Helpers
{
    //public class ProcessAuthorizationHandler : IAuthorizationHandler
    //{
    //    public Task HandleAsync(AuthorizationHandlerContext context)
    //    {

    //        var pendingRequirements = context.PendingRequirements.ToList();

    //        foreach (var requirement in pendingRequirements)
    //        {
    //            if (requirement is ReadPermission)
    //            {
    //                if (IsOwner(context.User, context.Resource)
    //                    || IsSponsor(context.User, context.Resource))
    //                {
    //                    context.Succeed(requirement);
    //                }
    //            }
    //            else if (requirement is EditPermission || requirement is DeletePermission)
    //            {
    //                if (IsOwner(context.User, context.Resource))
    //                {
    //                    context.Succeed(requirement);
    //                }
    //            }
    //        }

    //        return Task.CompletedTask;
    //    }

    //    private static bool IsOwner(ClaimsPrincipal user, object? resource)
    //    {
    //        // Code omitted for brevity
    //        return true;
    //    }

    //    private static bool IsSponsor(ClaimsPrincipal user, object? resource)
    //    {
    //        // Code omitted for brevity
    //        return true;
    //    }
    //}
    public class ProcessAuthorizationHandler { 
    }
}
