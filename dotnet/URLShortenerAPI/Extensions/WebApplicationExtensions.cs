using Microsoft.AspNetCore.Mvc;
using URLShortenerAPI.Filters;
using URLShortenerAPI.Interfaces;

namespace URLShortenerAPI.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // POST /url
        app.MapPost("url/", async ([FromBody] string url, IUrlShortenerService urlShortenerService, CancellationToken cancellationToken) =>
        {
            var shortUrl = await urlShortenerService.CreateShortUrl(url, cancellationToken);
            return Results.Ok(new { ShortUrl = shortUrl });
        })
        .WithName("CreateUrlShortener")
        .WithOpenApi()
        .AddEndpointFilter<UrlEndpointFilter>();

        // GET url/{shortlUrl}
        app.MapGet("{shortCode}", async ([FromRoute] string shortCode, IUrlShortenerService urlShortenerService, CancellationToken cancellationToken) =>
        {
            var url = await urlShortenerService.GetSourceUrl(shortCode, cancellationToken);

            if (string.IsNullOrEmpty(url))
            {
                return Results.BadRequest("Failed to get the URL!");
            }

            return Results.Redirect(url);
        })
        .WithName("GetUrlSource")
        .WithOpenApi();

        return app;
    }
}
