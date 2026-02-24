using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Enums;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Shared.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class PlayerRepository(FootballTennisDbContext context) : IPlayerRepository
{
    public async Task<IReadOnlyList<PlayerStatsReadModel>> GetPlayersModelsStatsAsync(CancellationToken ct)
    {
        return await context.Players
            .AsNoTracking()
            .Select(x => new PlayerStatsReadModel
            {
                PlayerId = x.Id,
                FullName = x.FullName,
                TournamentsCount = x.TeamPlayers.Count,
                FirstCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 1),
                SecondCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 2),
                ThirdCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 3)
            })
            .OrderByDescending(x => x.FirstCount)
            .ToListAsync(ct);
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
