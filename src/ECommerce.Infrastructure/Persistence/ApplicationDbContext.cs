using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerce.Infrastructure.Persistence;

/// <summary>
/// EF Core database context — the single gateway to PostgreSQL. Exposes the entity sets and
/// applies every <see cref="IEntityTypeConfiguration{TEntity}"/> mapping found in this assembly.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-discovers ProductConfiguration, CategoryConfiguration, etc. — no manual registration.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Stamps audit timestamps automatically so callers never have to remember to set them.
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAtUtc = DateTime.UtcNow;
            else if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
