using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Shared.Enums;
using FootballTennis.Shared.Pagination;
using FootballTennis.Shared.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class TournamentRepository(FootballTennisDbContext context) : ITournamentRepository
{
    public async Task<(int, IReadOnlyList<TournamentListItemReadModel>)> GetAllTournamentsAsync(PagedRequest request, CancellationToken ct)
    {
        var tournaments = context.Tournaments
            .AsNoTracking()
            .Select(x => new TournamentListItemReadModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Date = x.Date,
                Status = x.Status,
                TeamPlayersCount = x.TeamPlayersCount,
                TeamsCount = x.Teams.Count,
                MatchesCount = x.Matches.Count,
                WinnerName = x.Status == Status.Finished ? x.Teams.Where(x => x.Position == 1).Select(x => x.Name).FirstOrDefault() : null
            });

        if (!string.IsNullOrWhiteSpace(request.Search))
            tournaments = tournaments.Where(x => x.Address.Contains(request.Search) || x.Name.Contains(request.Search));

        var totalTournamentsCount = await tournaments.CountAsync(ct);

        tournaments = request.SortBy switch
        {
            "Name" => request.Desc
                ? tournaments.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.Name).ThenBy(x => x.Id),

            "Address" => request.Desc
                ? tournaments.OrderByDescending(x => x.Address).ThenByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.Address).ThenBy(x => x.Id),

            "Date" => request.Desc
                ? tournaments.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.Date).ThenBy(x => x.Id),

            "Status" => request.Desc
                ? tournaments.OrderByDescending(x => x.Status).ThenByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.Status).ThenBy(x => x.Id),

            "TeamPlayersCount" => request.Desc
                ? tournaments.OrderByDescending(x => x.TeamPlayersCount).ThenByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.TeamPlayersCount).ThenBy(x => x.Id),

            _ => request.Desc
                ? tournaments.OrderByDescending(x => x.Id)
                : tournaments.OrderBy(x => x.Id)
        };

        var allTournaments = await tournaments
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (totalTournamentsCount, allTournaments);
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
        return await context
            .Tournaments
            .Include(x => x.Teams)
            .FirstOrDefaultAsync(x => x.Id == tournamentId, ct);
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

    public async Task<bool> IsTournamentStatusScheduled(int tournamentId, CancellationToken ct)
    {
        return await context.Tournaments.AnyAsync(x => x.Id == tournamentId && x.Status == Status.Scheduled, ct);
    }
}
