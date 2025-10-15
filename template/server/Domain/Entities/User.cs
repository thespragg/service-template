namespace Server.Domain.Entities;

internal sealed record User
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Identifier { get; init; }
}
