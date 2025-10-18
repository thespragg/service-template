using Server.Http.Routes.Users.Handlers;

namespace Server.Http.Routes.Users;

internal static class UserRoutes
{
    internal static IEndpointRouteBuilder MapUserRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("users/current", GetCurrentUser.Handle);
        return builder;
    }

   
}