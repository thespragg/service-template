using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Contracts;
using Server.Persistence;
using Server.Persistence.Schemas;

namespace Server.Http.Middleware;

internal sealed class UserBindingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        DataContext dbContext,
        IUserCache userCache
    )
    {
        var isAuthed = context.User.Identity?.IsAuthenticated ?? true;
        if (isAuthed)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var userDto = await userCache.GetOrAddAsync(userId, async () =>
            {
                var user = await dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Identifier == userId);

                if (user != null) return user.ToDto();

                var id = Guid.NewGuid();
                var email = context.User.FindFirstValue(ClaimTypes.Email)!;

                user = new UserSchema
                {
                    Id = id,
                    Identifier = userId,
                    Email = email
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return user.ToDto();
            });

            context.Items["User"] = userDto;
        }

        await next(context);
    }
}