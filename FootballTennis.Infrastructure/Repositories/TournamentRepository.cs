using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Enums;
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
            .OrderByDescending(x => x.Status == Status.InProgress)
            .ThenBy(x => x.Date)
            .ToListAsync(ct);
    }
    public void AddTournament(Tournament tournament)
    {
        context.Tournaments.Add(tournament);
    }

    public async Task<bool> ExistsTournamentWithSameName(string tournamentName, CancellationToken ct)
    {
        return await context.Tournaments.AnyAsync(x => x.Name == tournamentName, ct);
    }

    public async Task<bool> ExistsWithSameNameExceptId(int tournamentId, string tournamentName, CancellationToken ct)
    {
        return await context.Tournaments.AnyAsync(x => x.Id != tournamentId && x.Name == tournamentName, ct);
    }

    public async Task<Tournament?> GetTournamentByIdAsync(int tournamentId, CancellationToken ct)
    {
        return await context.Tournaments.FirstOrDefaultAsync(x => x.Id == tournamentId, ct);
    }

    public void DeleteTournament(Tournament tournament)
    {
        context.Tournaments.Remove(tournament);
    }

    public async Task<Tournament?> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct)
    {
        return await context.Tournaments
            .AsNoTracking()
                .Include(x => x.Matches)
                    .ThenInclude(x => x.TeamOne)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.TeamTwo)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Sets)
                .Include(x => x.Teams)
                    .ThenInclude(x => x.TeamPlayers)
                        .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.Id == tournamentId, ct);
    }
}
