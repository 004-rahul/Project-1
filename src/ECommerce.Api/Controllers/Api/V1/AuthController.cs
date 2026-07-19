using System.Security.Claims;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers.Api.V1;

/// <summary>
/// REST auth for API clients (web SPA / mobile). Issues JWT access tokens + rotating refresh tokens
/// against the same Identity user store the MVC cookie login uses.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identity;

    public AuthController(IIdentityService identity) => _identity = identity;

    /// <summary>Create a customer account and return the first token pair.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _identity.RegisterAsync(request, cancellationToken);
        return result.Succeeded ? Ok(result) : BadRequest(new { error = result.Error });
    }

    /// <summary>Exchange credentials for a token pair.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _identity.LoginAsync(request, cancellationToken);
        return result.Succeeded ? Ok(result) : Unauthorized(new { error = result.Error });
    }

    /// <summary>Exchange a valid refresh token for a new token pair (the old one is revoked).</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request, CancellationToken cancellationToken)
    {
        var result = await _identity.RefreshAsync(request.RefreshToken, cancellationToken);
        return result.Succeeded ? Ok(result) : Unauthorized(new { error = result.Error });
    }

    /// <summary>Revoke a refresh token.</summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request, CancellationToken cancellationToken)
    {
        await _identity.LogoutAsync(request.RefreshToken, cancellationToken);
        return NoContent();
    }

    /// <summary>Returns the caller's identity — proves the access token works. Requires a bearer token.</summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"),
        email = User.FindFirstValue(ClaimTypes.Email),
        name = User.Identity?.Name,
        roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
    });
}
