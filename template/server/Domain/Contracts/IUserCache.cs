using Server.Domain.Entities;

namespace Server.Domain.Contracts;

internal interface IUserCache
{
    Task<User> GetOrAddAsync(string identifier, Func<Task<User>> factory);
    void Invalidate(string identifier);
}
