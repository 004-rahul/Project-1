using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

/// <summary>
/// Serves the storefront's public HTML pages (rendered as Razor views).
/// API controllers live alongside this one but return JSON instead of views.
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
