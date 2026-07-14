namespace ECommerce.Domain.Common;

/// <summary>
/// Base type for all persisted entities. Centralises the surrogate key and audit
/// timestamps so every entity gets them for free (DRY) and they are stored consistently.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Surrogate primary key (database-generated identity).</summary>
    public int Id { get; set; }

    /// <summary>UTC timestamp set when the row is first created.</summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>UTC timestamp set on every update; null until the row is first modified.</summary>
    public DateTime? UpdatedAtUtc { get; set; }
}
