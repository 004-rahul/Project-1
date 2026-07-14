namespace ECommerce.Domain.Enums;

/// <summary>
/// Lifecycle state of a product in the catalogue. Stored as an integer so the values
/// are stable even if the names change; do not renumber existing members.
/// </summary>
public enum ProductStatus
{
    /// <summary>Created but not yet visible to shoppers.</summary>
    Draft = 0,

    /// <summary>Published and available for purchase.</summary>
    Active = 1,

    /// <summary>Published but temporarily unavailable (no stock).</summary>
    OutOfStock = 2,

    /// <summary>Permanently removed from sale; kept for order history.</summary>
    Discontinued = 3
}
