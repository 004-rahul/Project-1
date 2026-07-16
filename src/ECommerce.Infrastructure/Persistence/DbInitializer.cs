using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

/// <summary>
/// Applies any pending migrations and seeds a starter catalogue the first time the app runs, so a
/// fresh clone shows real data immediately with no manual database steps.
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        // Create the database / apply pending migrations. Convenient for dev; in production you would
        // run migrations as an explicit deploy step instead of on startup.
        await context.Database.MigrateAsync(cancellationToken);

        // Only seed an empty database — never duplicate on restart.
        if (await context.Categories.AnyAsync(cancellationToken))
            return;

        var laptops = new Category
        {
            Name = "Laptops",
            Slug = "laptops",
            Description = "Portable computers.",
            Products = new List<Product>
            {
                new() { Name = "Dell XPS 13", Sku = "LAP-DELL-XPS13", Price = 1299.00m, StockQuantity = 12, Status = ProductStatus.Active, Description = "13-inch ultrabook with an InfinityEdge display." },
                new() { Name = "MacBook Air M3", Sku = "LAP-MAC-AIRM3", Price = 1199.00m, StockQuantity = 8, Status = ProductStatus.Active, Description = "Fanless laptop powered by the Apple M3 chip." },
                new() { Name = "Lenovo ThinkPad X1 Carbon", Sku = "LAP-LEN-X1C", Price = 1599.00m, StockQuantity = 0, Status = ProductStatus.OutOfStock, Description = "Business ultrabook with a legendary keyboard." }
            }
        };

        var accessories = new Category
        {
            Name = "Accessories",
            Slug = "accessories",
            Description = "Peripherals and add-ons.",
            Products = new List<Product>
            {
                new() { Name = "Logitech MX Master 3S", Sku = "ACC-LOGI-MX3S", Price = 99.00m, StockQuantity = 40, Status = ProductStatus.Active, Description = "Ergonomic wireless mouse." },
                new() { Name = "Keychron K2 Keyboard", Sku = "ACC-KEY-K2", Price = 89.00m, StockQuantity = 25, Status = ProductStatus.Active, Description = "Compact mechanical keyboard." },
                new() { Name = "USB-C 7-in-1 Hub", Sku = "ACC-HUB-USBC7", Price = 49.00m, StockQuantity = 60, Status = ProductStatus.Active, Description = "Expand one USB-C port into seven." }
            }
        };

        context.Categories.AddRange(laptops, accessories);
        await context.SaveChangesAsync(cancellationToken);
    }
}
