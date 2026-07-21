using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace ECommerce.Api.Models;

/// <summary>Backs the customer self-registration form — full profile, not just credentials.</summary>
public class RegisterViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [Display(Name = "Address")]
    public string AddressLine { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "State / Province")]
    public string State { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "The password must be at least 8 characters.")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }

    /// <summary>External providers (e.g. Google) available for sign-up — empty when none are configured.</summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();
}
