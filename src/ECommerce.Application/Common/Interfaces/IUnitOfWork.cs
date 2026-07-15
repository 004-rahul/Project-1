namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Commits all changes staged through the repositories in a single transaction (one round-trip).
/// Separating "stage" (repository) from "commit" (unit of work) lets a service group several
/// writes into one atomic save and keeps the Application layer free of any EF Core dependency.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persists every staged change; returns the number of affected rows.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
