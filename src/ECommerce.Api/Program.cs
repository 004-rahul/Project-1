using ECommerce.Infrastructure;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// MVC pages (conventional routing) — the UI you see in the browser.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Attribute-routed API controllers (e.g. /api/products) return JSON.
app.MapControllers();

app.Run();
