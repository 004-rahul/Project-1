using ECommerce.Api.Models;
using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Areas.Admin.Controllers;

/// <summary>Admin dashboard landing page. The entire Admin area is restricted to the Admin role.</summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("admin")]
public class DashboardController : Controller
{
    private readonly IProductService _products;

    public DashboardController(IProductService products) => _products = products;

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _products.GetAllAsync(cancellationToken);
        return View(new AdminDashboardViewModel { ProductCount = products.Count });
    }
}
