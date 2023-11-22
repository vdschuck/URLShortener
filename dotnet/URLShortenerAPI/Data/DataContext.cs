using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<MyUrlShortener> MyUrlShorteners => Set<MyUrlShortener>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyUrlShortener>(builder =>
        {
            builder.ToTable("UrlShortener")
                .HasKey(x => x.ShortUrl);

            builder.Property(x => x.SourceUrl)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.ShortUrl)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.ShortUrl)
                .IsUnique(true);

            builder.HasIndex(x => x.SourceUrl)
                .IsUnique(true);
        });

        base.OnModelCreating(modelBuilder);
    }
}
