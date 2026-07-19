using ECommerce.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

/// <summary>Maps <see cref="RefreshToken"/> — indexed by token string for fast lookup on refresh.</summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Token).HasMaxLength(200).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
        builder.HasIndex(t => t.Token);
        builder.Ignore(t => t.IsActive); // computed, not persisted
    }
}
