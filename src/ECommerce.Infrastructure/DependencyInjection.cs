using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Products;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

/// <summary>
/// One place to register every Infrastructure service (database now; Redis and RabbitMQ later).
/// The API project just calls <c>AddInfrastructure(...)</c> and stays unaware of the wiring details.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

        // Data-access abstractions — scoped so each request shares one DbContext instance.
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Application services.
        services.AddScoped<IProductService, ProductService>();

        // Auth: JWT access + refresh token services (used by the REST API).
        services.AddScoped<JwtTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
