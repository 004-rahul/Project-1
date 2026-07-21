using System.Security.Claims;
using ECommerce.Api.Models;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

/// <summary>
/// Cookie-based auth for the browser (MVC pages): customer self-registration, login, logout, and
/// external (Google) sign-in. API clients use the JWT endpoints under /api/v1/auth instead.
/// </summary>
[Route("account")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("register")]
    public async Task<IActionResult> Register(string? returnUrl = null)
        => View(new RegisterViewModel { ReturnUrl = returnUrl, ExternalLogins = await GetExternalLoginsAsync() });

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ExternalLogins = await GetExternalLoginsAsync();
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.Phone,
            AddressLine = model.AddressLine,
            City = model.City,
            State = model.State,
            PostalCode = model.PostalCode,
            Country = model.Country
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            model.ExternalLogins = await GetExternalLoginsAsync();
            return View(model);
        }

        // Every self-registered account is a customer; admins are provisioned separately.
        await _userManager.AddToRoleAsync(user, DbInitializer.CustomerRole);
        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(model.ReturnUrl ?? Url.Action("Index", "Home")!);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string? returnUrl = null)
        => View(new LoginViewModel { ReturnUrl = returnUrl, ExternalLogins = await GetExternalLoginsAsync() });

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ExternalLogins = await GetExternalLoginsAsync();
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
                return LocalRedirect(model.ReturnUrl ?? Url.Action("Index", "Home")!);
        }

        // Same message whether the email or the password was wrong — don't reveal which.
        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        model.ExternalLogins = await GetExternalLoginsAsync();
        return View(model);
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("denied")]
    public IActionResult Denied() => View();

    // --- External (Google) sign-in -------------------------------------------------------------

    /// <summary>Kicks off the OAuth challenge for the chosen provider (e.g. "Google").</summary>
    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    /// <summary>
    /// Where the provider redirects back. Signs the user in if the external login is already linked;
    /// otherwise creates a local account entry from the provider's profile and links it.
    /// </summary>
    [HttpGet("external-callback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Action("Index", "Home")!;

        if (remoteError is not null)
        {
            TempData["ExternalLoginError"] = $"Error from the external provider: {remoteError}";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            TempData["ExternalLoginError"] = "Could not read the external login information. Please try again.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        // Already linked? Just sign in.
        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
            return LocalRedirect(returnUrl);

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            TempData["ExternalLoginError"] = "The external provider did not return an email address.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        // First time with this provider: create a local account entry, or link to an existing one.
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
            };

            var created = await _userManager.CreateAsync(user);
            if (!created.Succeeded)
            {
                TempData["ExternalLoginError"] = "Could not create a local account for this login.";
                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            await _userManager.AddToRoleAsync(user, DbInitializer.CustomerRole);
        }

        await _userManager.AddLoginAsync(user, info);
        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(returnUrl);
    }

    private async Task<IList<AuthenticationScheme>> GetExternalLoginsAsync()
        => (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
}
