
namespace URLShortenerAPI.Filters;

public class UrlEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.GetArgument<string>(0);
        var isValidUri = Uri.TryCreate(endpoint, UriKind.Absolute, out var uriResult);

        if (isValidUri)
        {
            return await next(context);
        }

        return Results.BadRequest("Url is not valid!");
    }
}