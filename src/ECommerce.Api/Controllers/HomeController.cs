using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

/// <summary>
/// Storefront home — shows the product catalogue to shoppers. The same host also serves the admin
/// pages and (later) a JSON API; this controller only renders customer-facing views.
/// </summary>
public class HomeController : Controller
{
    private readonly IProductService _products;

    public HomeController(IProductService products) => _products = products;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _products.GetAllAsync(cancellationToken);
        return View(products);
    }
}
