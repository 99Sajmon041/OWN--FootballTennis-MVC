using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Infrastructure.Repositories;
using FootballTennis.Infrastructure.SeedOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballTennis.Infrastructure.Extensions;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FootballTennisDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<DefaultSeeder>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
