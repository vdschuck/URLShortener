using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using URLShortenerAPI.Configurations;
using URLShortenerAPI.Data;
using URLShortenerAPI.Interfaces;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Repositories;

public class MyUrlShortenerRepository(IMemoryCache memoryCache, DataContext dbContext, IOptionsMonitor<ApplicationConfig> appConfig) : IMyUrlShortenerRepository
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly DataContext _dbContext = dbContext;
    private readonly IOptionsMonitor<ApplicationConfig> _appConfig = appConfig;

    public async Task Create(MyUrlShortener urlShortener, CancellationToken cancellationToken)
    {
        await _dbContext.MyUrlShorteners.AddAsync(urlShortener, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheExpiration = TimeSpan.FromHours(_appConfig.CurrentValue.CacheExpirationInHours);
        _memoryCache.Set(urlShortener.ShortUrl, urlShortener.SourceUrl, cacheExpiration);
    }

    public async Task<MyUrlShortener?> GetByShortUrl(string shortUrl, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(shortUrl, out string? sourceUrl) && sourceUrl != null)
        {
            return new MyUrlShortener(sourceUrl, shortUrl);
        }

        return await _dbContext.MyUrlShorteners.FirstOrDefaultAsync(x => x.ShortUrl.Equals(shortUrl), cancellationToken);
    }

    public async Task<MyUrlShortener?> GetBySourceUrl(string sourceUrl, CancellationToken cancellationToken) =>
        await _dbContext.MyUrlShorteners.FirstOrDefaultAsync(x => x.SourceUrl.Equals(sourceUrl), cancellationToken);
}
