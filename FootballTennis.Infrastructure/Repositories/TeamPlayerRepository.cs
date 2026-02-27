using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class TeamPlayerRepository(FootballTennisDbContext context) : ITeamPlayerRepository
{
    public async Task<bool> AlreadyExistsPlayersAtTournamentAsync(int tournamentId, List<int> playersId, CancellationToken ct)
    {
        return await context.TeamPlayers
            .AnyAsync(x => x.TournamentId == tournamentId && playersId.Contains(x.PlayerId), ct);
    }

    public async Task<bool> AlreadyExistsPlayersInOtherTeamAsync(int tournamentId, int teamId, List<int> playersId, CancellationToken ct)
    {
        return await context.TeamPlayers
            .AsNoTracking()
            .AnyAsync(x => x.TournamentId == tournamentId &&  x.TeamId != teamId && playersId.Contains(x.PlayerId), ct);
    }

    public async Task<List<TeamPlayer>> GetAllTeamPlayersAsync(int tournamentId,int id, CancellationToken ct)
    {
        return await context.TeamPlayers
            .AsNoTracking()
            .Include(x => x.Player)
            .Where(x => x.TournamentId == tournamentId && x.TeamId == id)
            .ToListAsync(ct);
    }

    public async Task<List<int>> GetAssignedPlayerIdsAsync(int tournamentId, CancellationToken ct)
    {
        return await context.TeamPlayers
            .AsNoTracking()
            .Where(x => x.TournamentId == tournamentId)
            .Select(x => x.PlayerId)
            .ToListAsync(ct);
    }
}
