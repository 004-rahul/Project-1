using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUnitOfWork"/>. The <see cref="ApplicationDbContext"/> is itself
/// a unit of work; this thin wrapper simply exposes its commit step through an Application-layer contract.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
