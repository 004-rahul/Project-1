namespace ECommerce.Application.Products;

/// <summary>
/// A product shaped for display. Flattens the category name and exposes the status as text, so views
/// and API responses never touch the EF entity directly.
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>Convenience flag for the UI — avoids repeating the stock check in every view.</summary>
    public bool InStock => StockQuantity > 0;
}
