using ECommerce.Application.Identity;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Auth use-cases for the REST API. Returns JWT access + refresh tokens so a mobile or SPA client can
/// authenticate against the same user store the web app uses.
/// </summary>
public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
}
