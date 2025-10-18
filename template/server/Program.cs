using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Contracts;
using Server.Domain.Services;
using Server.Http.Extensions;
using Server.Persistence;
using Server.Policies.Json;

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
        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<DataContext>(options =>
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
        );
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddOpenApi();
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IUserCache, UserCache>();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(new ScreamingSnakeCaseNamingPolicy())
            );
        });

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

        var group = app.MapGroup(string.Empty).RequireAuthorization();
        group.MapEndpoints();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.MigrateAsync();
        }
        await app.RunAsync("http://*:5000");
    }
}