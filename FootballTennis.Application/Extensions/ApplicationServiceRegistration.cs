using FootballTennis.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using FootballTennis.Application.Services.Implementations;
using FootballTennis.Application.Services.Interfaces;

namespace FootballTennis.Application.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(TournamentMappingProfile));

        services.AddScoped<ITournamentService, TournamentService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITeamService, TeamService>();

        return services;
    }
}
