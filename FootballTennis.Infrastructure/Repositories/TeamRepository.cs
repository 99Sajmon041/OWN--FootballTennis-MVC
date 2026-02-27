using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class TeamRepository(FootballTennisDbContext context) : ITeamRepository
{
    public void AddTeam(Team team)
    {
        context.Teams.Add(team);
    }

    public void DeleteTeam(Team team)
    {
        context.Teams.Remove(team);
    }

    public async Task<Team?> GetTeamByIdAsync(int id, CancellationToken ct)
    {
        return await context.Teams
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Team?> GetTeamByIdWithDetailsAsync(int tournamentId, int id, CancellationToken ct)
    {
        return await context.Teams
            .Include(x => x.TeamPlayers)
            .FirstOrDefaultAsync(x => x.TournamentId == tournamentId && x.Id == id, ct);
    }
}
