namespace ECommerce.Infrastructure.Identity;

/// <summary>
/// A server-side refresh token. Long-lived and stored so it can be rotated (a new one issued on each
/// use) and revoked (on logout) — unlike the short-lived, stateless JWT access token.
/// </summary>
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }

    /// <summary>Usable only while not revoked and not expired.</summary>
    public bool IsActive => RevokedAtUtc is null && DateTime.UtcNow < ExpiresAtUtc;
}
