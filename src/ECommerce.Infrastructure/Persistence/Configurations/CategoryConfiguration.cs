using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps <see cref="Category"/> to its table — column sizes, constraints, indexes, relationships.
/// Keeping mapping here (Fluent API) instead of attributes keeps the Domain layer POCO-clean.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(120).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(140).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(1000);

        // Slugs appear in storefront URLs, so they must be unique.
        builder.HasIndex(c => c.Slug).IsUnique();

        // One category has many products; block deleting a category that still has products.
        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
