using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class TournamentRepository(FootballTennisDbContext context) : ITournamentRepository
{
    public async Task<IReadOnlyList<Tournament>> GetAllTournamentsAsync(CancellationToken ct)
    {
        return await context.Tournaments
            .AsNoTracking()
            .Include(x => x.Matches)
            .Include(x => x.Teams)
            .OrderByDescending(x => x.Date)
            .ToListAsync(ct);
    }
}
