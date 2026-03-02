using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class MatchRepository(FootballTennisDbContext context) : IMatchRepository
{
    public async Task<bool> ExistsAnyForTournamentAsync(int tournamentId, CancellationToken ct)
    {
        return await context.Matches
            .AnyAsync(x => x.TournamentId == tournamentId, ct);
    }
}
