using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

/// <summary>
/// The application's user account. Extends <see cref="IdentityUser"/> so profile fields can be added
/// later without disturbing the authentication plumbing.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Optional display name shown in the UI (e.g. the header greeting).</summary>
    public string? FullName { get; set; }
}
