using ECommerce.Application.Products;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Application logic for the product catalogue — the entry point controllers call. Returns DTOs,
/// never entities, so the web layer stays decoupled from the domain model and EF Core.
/// </summary>
public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
