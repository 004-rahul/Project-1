using ECommerce.Api.Models;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Api.Areas.Admin.Controllers;

/// <summary>Admin product management (list / create / delete). Restricted to the Admin role.</summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("admin/products")]
public class ProductsController : Controller
{
    private readonly IProductService _products;
    private readonly ICategoryRepository _categories;

    public ProductsController(IProductService products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _products.GetAllAsync(cancellationToken);
        return View(products);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var form = new ProductFormViewModel { CategoryOptions = await BuildCategoryOptionsAsync(cancellationToken) };
        return View(form);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            form.CategoryOptions = await BuildCategoryOptionsAsync(cancellationToken);
            return View(form);
        }

        await _products.CreateAsync(new CreateProductRequest
        {
            Name = form.Name,
            Description = form.Description,
            Sku = form.Sku,
            Price = form.Price,
            Currency = form.Currency,
            StockQuantity = form.StockQuantity,
            CategoryId = form.CategoryId,
            Status = form.Status
        }, cancellationToken);

        TempData["Message"] = $"Product \"{form.Name}\" created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _products.DeleteAsync(id, cancellationToken);
        TempData["Message"] = "Product deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> BuildCategoryOptionsAsync(CancellationToken cancellationToken)
    {
        var categories = await _categories.ListAsync(cancellationToken);
        return categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
    }
}
