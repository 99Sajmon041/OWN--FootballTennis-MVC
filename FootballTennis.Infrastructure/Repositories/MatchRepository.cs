using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class MatchRepository(FootballTennisDbContext context) : IMatchRepository
{
    public async Task<bool> ExistsAnyForTournamentAsync(int tournamentId, CancellationToken ct)
    {
        return await context.Matches
            .AnyAsync(x => x.TournamentId == tournamentId, ct);
    }

    public async Task<Match?> GetMatchForUpdateAsync(int id, int tournamentId, CancellationToken ct)
    {
        return await context.Matches
            .Include(x => x.Sets)
            .Include(x => x.TeamOne)
            .Include(x => x.TeamTwo)
            .FirstOrDefaultAsync(x => x.TournamentId == tournamentId && x.Id == id && x.Status == Status.InProgress && x.Id == id, ct);
    }
}
