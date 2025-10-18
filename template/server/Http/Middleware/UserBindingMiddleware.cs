using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Contracts;
using Server.Domain.Enums;
using Server.Persistence;
using Server.Persistence.Schemas;
using Stripe;

namespace Server.Http.Middleware;

internal sealed class UserBindingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        DataContext dbContext,
        IUserCache userCache,
        IStripePaymentService paymentService
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
                    .Include(x => x.Subscription)
                    .FirstOrDefaultAsync(x => x.Identifier == userId);

                if (user != null) return user.ToDto();

                var id = Guid.NewGuid();
                var email = context.User.FindFirstValue(ClaimTypes.Email)!;

                var customer = await paymentService.CreateStripeCustomer(email, id);
                user = new UserSchema
                {
                    Id = id,
                    Identifier = userId,
                    Email = email,
                    StripeCustomerId = customer.Id,
                    Subscription = null! // Only temporarily null
                };
                var subscription = new SubscriptionSchema
                {
                    Id = Guid.NewGuid(),
                    Type = SubscriptionType.Free,
                    Status = SubscriptionStatus.Inactive,
                    ExternalSubscriptionId = string.Empty,
                    StartedAt = DateTime.UtcNow,
                    UserId = user.Id,
                    EndedAt = null
                };
                user = user with { Subscription = subscription };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return user.ToDto();
            });

            context.Items["User"] = userDto;
        }

        await next(context);
    }
}