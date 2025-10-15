using Server.Domain.Entities;
using Server.Persistence.Interfaces;

namespace Server.Persistence.Schemas;

internal sealed record UserSchema : IHasDto<User>
{
    public required Guid Id { get; init; }
    public required string Identifier { get; init; }
    public required string Email { get; init; }

    public User ToDto()
        => new()
        {
            Id = Id,
            Identifier = Identifier,
            Email = Email
        };
}