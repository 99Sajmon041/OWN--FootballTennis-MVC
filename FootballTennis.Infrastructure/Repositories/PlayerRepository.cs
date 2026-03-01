using FootballTennis.Domain.Entities;
using FootballTennis.Shared.Enums;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Shared.Pagination;
using FootballTennis.Shared.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class PlayerRepository(FootballTennisDbContext context) : IPlayerRepository
{
    public async Task<(int, IReadOnlyList<PlayerListItemReadModel>)> GetPlayersModelsStatsAsync(PagedRequest request, CancellationToken ct)
    {
        var players = context.Players
            .AsNoTracking()
            .Select(x => new PlayerListItemReadModel
            {
                PlayerId = x.Id,
                FullName = x.FullName,
                TournamentsCount = x.TeamPlayers.Count,
                FirstCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 1),
                SecondCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 2),
                ThirdCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 3)
            });

        if (!string.IsNullOrWhiteSpace(request.Search))
            players = players.Where(x => x.FullName.Contains(request.Search));

        var totalPlayersCount = await players.CountAsync(ct);

        players = request.SortBy switch
        {
            "FullName" => request.Desc
                ? players.OrderByDescending(x => x.FullName).ThenByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.FullName).ThenBy(x => x.PlayerId),

            "TournamentsCount" => request.Desc
                ? players.OrderByDescending(x => x.TournamentsCount).ThenByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.TournamentsCount).ThenBy(x => x.PlayerId),

            "FirstCount" => request.Desc
                ? players.OrderByDescending(x => x.FirstCount).ThenByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.FirstCount).ThenBy(x => x.PlayerId),

            "SecondCount" => request.Desc
                ? players.OrderByDescending(x => x.SecondCount).ThenByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.SecondCount).ThenBy(x => x.PlayerId),

            "ThirdCount" => request.Desc
                ? players.OrderByDescending(x => x.ThirdCount).ThenByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.ThirdCount).ThenBy(x => x.PlayerId),

            _ => request.Desc
                ? players.OrderByDescending(x => x.PlayerId)
                : players.OrderBy(x => x.PlayerId)
        };

        var allPlayers = await players
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (totalPlayersCount, allPlayers);
    }

    public async Task AddPlayerAsync(Player player, CancellationToken ct)
    {
        await context.Players.AddAsync(player, ct);
    }

    public async Task<bool> ExistsPlayerWithSameNameAsync(string fullName, CancellationToken ct)
    {
        fullName = fullName.Trim();
        return await context.Players.AnyAsync(x => x.FullName == fullName, ct);
    }

    public async Task<bool> ExistsWithSameNameExceptId(int playerId, string playerName, CancellationToken ct)
    {
        return await context.Players.AnyAsync(x => x.FullName == playerName && x.Id != playerId, ct);
    }

    public void DeletePlayer(Player player)
    {
        context.Players.Remove(player);
    }

    public async Task<Player?> GetPlayerByIdAsync(int playerId, CancellationToken ct)
    {
        return await context.Players.FirstOrDefaultAsync(x => x.Id == playerId, ct);
    }

    public async Task<List<Player>> GetAllPlayersAsync(CancellationToken ct)
    {
        return await context.Players
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);
    }
}
