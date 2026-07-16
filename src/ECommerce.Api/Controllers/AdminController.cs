using ECommerce.Api.Models;
using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

/// <summary>Admin dashboard landing page. The entire admin area is restricted to the Admin role.</summary>
[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IProductService _products;

    public AdminController(IProductService products) => _products = products;

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _products.GetAllAsync(cancellationToken);
        return View(new AdminDashboardViewModel { ProductCount = products.Count });
    }
}
