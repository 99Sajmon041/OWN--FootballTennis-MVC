using FootballTennis.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace FootballTennis.Application.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(TournamentMappingProfile));
        return services;
    }
}
