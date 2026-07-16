using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// MVC (Razor views) + API controllers share this single host:
//   - conventional routes render HTML pages (the storefront UI)
//   - attribute-routed [ApiController]s under /api return JSON
builder.Services.AddControllersWithViews();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Infrastructure services (SQL Server via EF Core).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
builder.Services.AddInfrastructure(connectionString);

// ASP.NET Core Identity — cookie-based auth for the MVC pages, backed by our SQL Server store.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/denied";
});

var app = builder.Build();

// Create/migrate the database and seed starter data on startup, so a fresh clone just runs.
// (Convenient for dev; a production deploy would run migrations as an explicit step instead.)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.InitializeAsync(db, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Database initialization failed. The app will start, but data features " +
            "may not work until the database is reachable.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC pages (conventional routing) — the UI you see in the browser.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Attribute-routed API controllers (e.g. /api/products) return JSON.
app.MapControllers();

app.Run();
