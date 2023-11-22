using Microsoft.Extensions.Options;
using URLShortenerAPI.Configurations;
using URLShortenerAPI.Interfaces;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Services;

public class UrlShortenerService(ILogger<UrlShortenerService> logger, IOptionsMonitor<ApplicationConfig> appConfig, IMyUrlShortenerRepository repository) : IUrlShortenerService
{
    private readonly ILogger<UrlShortenerService> _logger = logger;
    private readonly IOptionsMonitor<ApplicationConfig> _appConfig = appConfig;
    private readonly IMyUrlShortenerRepository _repository = repository;

    public async Task<string> CreateShortUrl(string sourceUrl, CancellationToken cancellationToken)
    {
        var myUrl = await _repository.GetBySourceUrl(sourceUrl, cancellationToken);

        if (myUrl != null)
        {
            _logger.LogInformation("Url has already been registered {url}", sourceUrl);
            return myUrl.ShortUrl;
        }

        var shortUrl = ShortUrlBuilder(sourceUrl);
        myUrl = new MyUrlShortener(sourceUrl, shortUrl);

        await _repository.Create(myUrl, cancellationToken);

        _logger.LogInformation("Create a new short url {url}", myUrl.ShortUrl);

        return myUrl.ShortUrl;
    }

    public async Task<string> GetSourceUrl(string shortCode, CancellationToken cancellationToken)
    {
        var shortUrl = BuildShortUrl(shortCode);
        var myUrl = await _repository.GetByShortUrl(shortUrl, cancellationToken);

        if (myUrl != null)
        {
            _logger.LogInformation("Getting source url {url}", myUrl.SourceUrl);
            return myUrl.SourceUrl;
        }

        _logger.LogInformation("Short url does not exist {url}", shortUrl);
        return string.Empty;
    }

    private string ShortUrlBuilder(string url)
    {
        var shortCode = string.Format("{0:X}", url.GetHashCode());
        return $"{_appConfig.CurrentValue.BaseUrl}/{shortCode}";
    }

    private string BuildShortUrl(string shortCode)
    {
        return $"{_appConfig.CurrentValue.BaseUrl}/{shortCode}";
    }
}
