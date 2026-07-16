using ECommerce.Domain.Enums;

namespace ECommerce.Application.Products;

/// <summary>
/// The data needed to create a product. A plain input DTO — input validation lives in the web
/// view model so this stays a clean contract between the web and application layers.
/// </summary>
public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
