using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Identity;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Identity;

/// <summary>
/// Implements the auth use-cases for the REST API: register, login, refresh (with rotation), and
/// logout (revocation). Backed by the same Identity user store the MVC cookie login uses.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenService _tokens;
    private readonly JwtSettings _settings;
    private readonly ApplicationDbContext _db;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        JwtTokenService tokens,
        IOptions<JwtSettings> settings,
        ApplicationDbContext db)
    {
        _userManager = userManager;
        _tokens = tokens;
        _settings = settings.Value;
        _db = db;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            return AuthResult.Fail("An account with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.Phone
        };

        var created = await _userManager.CreateAsync(user, request.Password);
        if (!created.Succeeded)
            return AuthResult.Fail(string.Join(" ", created.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, DbInitializer.CustomerRole);
        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return AuthResult.Fail("Invalid email or password.");

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
        if (stored is null || !stored.IsActive)
            return AuthResult.Fail("Invalid or expired refresh token.");

        // Rotate: revoke the presented token, then issue a brand-new pair.
        stored.RevokedAtUtc = DateTime.UtcNow;

        var user = await _userManager.FindByIdAsync(stored.UserId);
        if (user is null)
            return AuthResult.Fail("User no longer exists.");

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
        if (stored is not null && stored.IsActive)
        {
            stored.RevokedAtUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<AuthResult> IssueTokensAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, expiresAt) = _tokens.CreateAccessToken(user, roles);
        var refreshToken = _tokens.CreateRefreshToken();

        _db.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_settings.RefreshTokenDays)
        });
        await _db.SaveChangesAsync(cancellationToken);

        return AuthResult.Success(accessToken, refreshToken, expiresAt, new UserInfo
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Roles = roles.ToList()
        });
    }
}
