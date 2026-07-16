using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerce.Infrastructure.Persistence;

/// <summary>
/// EF Core database context — the single gateway to SQL Server. Extends <see cref="IdentityDbContext{TUser}"/>
/// so ASP.NET Core Identity's user and role tables live alongside the catalogue tables.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Base call first so Identity's own entity mappings are applied.
        base.OnModelCreating(modelBuilder);

        // Then our configurations (ProductConfiguration, CategoryConfiguration, etc.).
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
