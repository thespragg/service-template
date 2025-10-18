using Server.Http.Routes;
using Server.Http.Routes.Payments;
using Server.Http.Routes.Users;

namespace Server.Http.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapEndpoints(this IEndpointRouteBuilder builder)
        => builder
            .MapUserRoutes();
}
