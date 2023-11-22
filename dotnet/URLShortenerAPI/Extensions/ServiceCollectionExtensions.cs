using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Configurations;
using URLShortenerAPI.Data;
using URLShortenerAPI.Interfaces;
using URLShortenerAPI.Repositories;
using URLShortenerAPI.Services;

namespace URLShortenerAPI.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection services)
    {
        services.AddTransient<IUrlShortenerService, UrlShortenerService>();
        services.AddTransient<IMyUrlShortenerRepository, MyUrlShortenerRepository>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, DataContext>();
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
        });

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationConfig>(configuration.GetSection("Application"));

        return services;
    }
}

