using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Data-access contract for <see cref="Product"/>. Lives in the Application layer so services depend
/// on this abstraction — not on EF Core — which keeps the domain testable and the ORM swappable.
/// Write methods only stage changes; call <see cref="IUnitOfWork.SaveChangesAsync"/> to commit.
/// </summary>
public interface IProductRepository
{
    /// <summary>Fetches a single product (with its category) for reads or edits. Null if not found.</summary>
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Fetches a product by its unique SKU. Null if not found.</summary>
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

    /// <summary>Returns one page of products, ordered by id. Read-only (untracked) for speed.</summary>
    Task<IReadOnlyList<Product>> ListAsync(int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>Total product count — used to build pagination metadata.</summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>True if a product already uses this SKU — lets services reject duplicates up front.</summary>
    Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken = default);

    /// <summary>Stages a new product for insertion.</summary>
    Task AddAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>Stages an existing product for update.</summary>
    void Update(Product product);

    /// <summary>Stages a product for deletion.</summary>
    void Remove(Product product);
}
