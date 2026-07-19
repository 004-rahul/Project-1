using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers.Api.V1;

/// <summary>
/// Public product catalogue REST API (JSON). The web app and a future mobile client both go through
/// the same <see cref="IProductService"/> — so there's one source of business logic, no duplication.
/// Read-only for now; write endpoints require auth (JWT), added in the next step.
/// </summary>
[ApiController]
[Route("api/v1/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;

    public ProductsController(IProductService products) => _products = products;

    /// <summary>Returns every catalogue product.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _products.GetAllAsync(cancellationToken));

    /// <summary>Returns a single product by id, or 404 if it does not exist.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }
}
