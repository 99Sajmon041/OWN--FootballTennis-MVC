using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class TeamRepository(FootballTennisDbContext context) : ITeamRepository
{
    public void AddTeamAsync(Team team)
    {
        context.Teams.Add(team);
    }
}
