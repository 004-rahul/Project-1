using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

/// <summary>
/// The application's user account. Extends <see cref="IdentityUser"/> with the customer profile
/// captured at registration (name + address). Email/phone live on the base <see cref="IdentityUser"/>.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Shipping / contact address captured at sign-up.
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    /// <summary>Display name shown in the UI and the JWT "name" claim — derived, not stored.</summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();
}
