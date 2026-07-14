using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps <see cref="Product"/> to its table — column sizes, money precision, enum storage, indexes.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(4000);
        builder.Property(p => p.Sku).HasMaxLength(64).IsRequired();

        // Money: fixed precision (18,2), never a floating-point column type.
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();

        // Persist the enum as its integer value (matches the stable values on ProductStatus).
        builder.Property(p => p.Status).HasConversion<int>();

        // SKU is a unique business key; index CategoryId for fast "products in a category" queries.
        builder.HasIndex(p => p.Sku).IsUnique();
        builder.HasIndex(p => p.CategoryId);
    }
}
