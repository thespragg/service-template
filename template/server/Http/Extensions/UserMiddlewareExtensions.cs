using Server.Domain.Entities;
using Server.Http.Middleware;

namespace Server.Http.Extensions;

public static class UserMiddlewareExtensions
{
    public static IApplicationBuilder UseUserBinding(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UserBindingMiddleware>();
    }

    public static IServiceCollection AddUserBinding(this IServiceCollection services)
    {
        services.AddScoped<User>(sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            return httpContext?.Items["User"] as User
                   ?? throw new InvalidOperationException("User not bound to context.");
        });

        return services;
    }
}
