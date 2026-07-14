using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

/// <summary>
/// A sellable item in the catalogue. Belongs to exactly one <see cref="Category"/>.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>Display name of the product.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional marketing description.</summary>
    public string? Description { get; set; }

    /// <summary>Stock Keeping Unit — a unique business identifier for inventory (e.g. "LAP-DELL-XPS13").</summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>Unit price. Uses <see cref="decimal"/> (never double/float) to avoid rounding errors on money.</summary>
    public decimal Price { get; set; }

    /// <summary>ISO 4217 currency code the price is expressed in (e.g. "USD").</summary>
    public string Currency { get; set; } = "USD";

    /// <summary>Units currently in stock.</summary>
    public int StockQuantity { get; set; }

    /// <summary>Current lifecycle state of the product.</summary>
    public ProductStatus Status { get; set; } = ProductStatus.Draft;

    /// <summary>Foreign key to the owning category.</summary>
    public int CategoryId { get; set; }

    /// <summary>The owning category (navigation property).</summary>
    public Category? Category { get; set; }
}
