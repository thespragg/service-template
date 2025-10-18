using Microsoft.Extensions.Caching.Memory;
using Server.Domain.Contracts;
using Server.Domain.Entities;

namespace Server.Domain.Services;

internal sealed class UserCache(IMemoryCache cache) : IUserCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<User> GetOrAddAsync(string userId, Func<Task<User>> factory)
    {
        if (cache.TryGetValue(userId, out User? cachedUser))
            return cachedUser!;

        var user = await factory();
        cache.Set(userId, user, CacheDuration);
        return user;
    }

    public void Invalidate(string userId)
    {
        cache.Remove(userId);
    }
}
