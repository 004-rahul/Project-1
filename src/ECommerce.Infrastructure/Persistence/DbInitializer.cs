using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

/// <summary>
/// Applies pending migrations and seeds the essentials on first run: the roles and one admin account,
/// so you can sign in to a fresh database. The product catalogue starts empty — you add real products
/// through the admin UI.
/// </summary>
public static class DbInitializer
{
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    private const string AdminEmail = "admin@shop.local";
    private const string AdminPassword = "Admin#12345";

    public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        CancellationToken cancellationToken = default)
    {
        // Create the database / apply pending migrations. Convenient for dev; a production deploy
        // would run migrations as an explicit step instead of on startup.
        await context.Database.MigrateAsync(cancellationToken);

        // Ensure the roles exist.
        foreach (var role in new[] { AdminRole, CustomerRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Ensure a default admin exists so a fresh database is immediately usable.
        if (await userManager.FindByEmailAsync(AdminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
                FirstName = "Store",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(admin, AdminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }
}
