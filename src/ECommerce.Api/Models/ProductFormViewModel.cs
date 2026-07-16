using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Api.Models;

/// <summary>
/// Backs the admin "create product" form: the validated input fields plus the category dropdown source.
/// Web-layer validation lives here (data annotations) so the Application DTO stays a clean contract.
/// </summary>
public class ProductFormViewModel
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Required, StringLength(64)]
    [Display(Name = "SKU")]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [Required, StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "USD";

    [Range(0, int.MaxValue)]
    [Display(Name = "Stock quantity")]
    public int StockQuantity { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    /// <summary>Dropdown options — not part of the posted data; repopulated by the controller.</summary>
    public IEnumerable<SelectListItem> CategoryOptions { get; set; } = Enumerable.Empty<SelectListItem>();
}
