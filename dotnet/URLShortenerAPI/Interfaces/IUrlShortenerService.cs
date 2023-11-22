namespace URLShortenerAPI.Interfaces;

public interface IUrlShortenerService
{
    Task<string> CreateShortUrl(string sourceUrl, CancellationToken cancellationToken);

    Task<string> GetSourceUrl(string shortCode, CancellationToken cancellationToken);
}
