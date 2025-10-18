using Microsoft.AspNetCore.Http.HttpResults;
using Server.Domain.Entities;

namespace Server.Http.Routes.Users.Handlers;

internal static class GetCurrentUser
{
    internal static Ok<User> Handle(
        User user,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(user);
}
