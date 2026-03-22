using System;
using Microsoft.Extensions.Caching.Memory;

namespace Jellyfin.Plugin.TelegramNotifier;

public class NotificationDeduplicator
{
    // Singleton via static
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private static readonly TimeSpan _ttl = TimeSpan.FromSeconds(30);

    public static bool ShouldSend(string fingerprint)
    {
        if (_cache.TryGetValue(fingerprint, out _))
        {
            return false; // duplicate
        }

        _cache.Set(fingerprint, true, _ttl); // add to cache
        return true; // send
    }
}