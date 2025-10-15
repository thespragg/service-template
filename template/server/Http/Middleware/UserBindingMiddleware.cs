using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Server.Persistence;
using Server.Persistence.Schemas;

namespace Server.Http.Middleware;

internal sealed class UserBindingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, DataContext dbContext)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var user = await dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Identifier == userId);

        if (user == null)
        {
            user = new UserSchema
            {
                Id = Guid.NewGuid(),
                Identifier = userId,
                Email = context.User.FindFirstValue(ClaimTypes.Email)!
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        context.Items["User"] = user.ToDto();
        await next(context);
    }
}