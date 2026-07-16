namespace ECommerce.Domain.Enums;

/// <summary>
/// Lifecycle state of a product in the catalogue. Stored as an integer so the values are stable even
/// if the names change; do not renumber existing members. Whether a product is "out of stock" is
/// derived from its stock quantity, so it is intentionally NOT a status here.
/// </summary>
public enum ProductStatus
{
    /// <summary>Created but not yet visible to shoppers.</summary>
    Draft = 0,

    /// <summary>Published and available for purchase.</summary>
    Active = 1,

    /// <summary>Was published, now hidden from the storefront (paused or retired) — not deleted.</summary>
    Inactive = 2
}
