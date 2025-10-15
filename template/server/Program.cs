using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;
using Server.Http.Extensions;
using Server.Persistence;

namespace Server;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors();
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Auth0:Authority"];
            options.Audience = builder.Configuration["Auth0:Audience"];
        });

        builder.Services.AddUserBinding();
        builder.Services
            .AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build()
            );

        builder.Services.AddDbContext<DataContext>(options =>
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
        );
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseCors(opts => opts
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)
        );

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseUserBinding();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.MigrateAsync();
        }
        
        app.MapGet("user", (User user) => TypedResults.Ok(user));
        await app.RunAsync("http://*:5000");
    }
}