using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Identity;

/// <summary>Sign-up payload for the REST API. Captures the user's profile, not just credentials.</summary>
public class RegisterRequest
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Login payload for the REST API.</summary>
public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Carries a refresh token for the refresh / logout endpoints.</summary>
public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>Result of an auth operation — tokens on success, or an error message.</summary>
public class AuthResult
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public DateTime? AccessTokenExpiresAtUtc { get; init; }
    public UserInfo? User { get; init; }

    public static AuthResult Fail(string error) => new() { Succeeded = false, Error = error };

    public static AuthResult Success(string accessToken, string refreshToken, DateTime expiresAt, UserInfo user) =>
        new()
        {
            Succeeded = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = expiresAt,
            User = user
        };
}

/// <summary>Minimal user info returned alongside tokens.</summary>
public class UserInfo
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = new List<string>();
}
