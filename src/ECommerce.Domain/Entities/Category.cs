using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

/// <summary>
/// A grouping of products (e.g. "Laptops", "Books"). One category has many products.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>Human-readable name shown to shoppers.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional longer description of the category.</summary>
    public string? Description { get; set; }

    /// <summary>URL-friendly identifier used in storefront links (e.g. "gaming-laptops").</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Products that belong to this category (navigation property).</summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
