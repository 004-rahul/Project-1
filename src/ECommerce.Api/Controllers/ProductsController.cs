using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

/// <summary>Storefront product pages shown to shoppers (the detail view).</summary>
public class ProductsController : Controller
{
    private readonly IProductService _products;

    public ProductsController(IProductService products) => _products = products;

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return NotFound();

        return View(product);
    }
}
