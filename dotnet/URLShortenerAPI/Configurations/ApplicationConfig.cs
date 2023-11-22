namespace URLShortenerAPI.Configurations;

public class ApplicationConfig
{
    public string BaseUrl { get; init; } = string.Empty;

    public int CacheExpirationInHours { get; init; }
}