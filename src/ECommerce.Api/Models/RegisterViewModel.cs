using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.Models;

/// <summary>Backs the customer self-registration form. Captures profile, not just credentials.</summary>
public class RegisterViewModel
{
    [Required, StringLength(150)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Phone (optional)")]
    public string? Phone { get; set; }

    [Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "The password must be at least 8 characters.")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
