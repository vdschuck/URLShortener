using URLShortenerAPI.Models;

namespace URLShortenerAPI.Interfaces;

public interface IMyUrlShortenerRepository
{
    Task Create(MyUrlShortener urlShortener, CancellationToken cancellationToken);

    Task<MyUrlShortener?> GetByShortUrl(string shortUrl, CancellationToken cancellationToken);

    Task<MyUrlShortener?> GetBySourceUrl(string sourceUrl, CancellationToken cancellationToken);
}
