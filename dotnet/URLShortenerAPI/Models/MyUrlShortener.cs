namespace URLShortenerAPI.Models;

public class MyUrlShortener(string sourceUrl, string shortUrl)
{
    public string SourceUrl { get; set; } = sourceUrl;

    public string ShortUrl { get; set; } = shortUrl;
}
