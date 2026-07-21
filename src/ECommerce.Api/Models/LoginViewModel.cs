using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace ECommerce.Api.Models;

/// <summary>Backs the sign-in form.</summary>
public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    /// <summary>External providers (e.g. Google) available for sign-in — empty when none are configured.</summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();
}
