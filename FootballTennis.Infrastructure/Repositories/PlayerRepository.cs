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
            .OrderBy(x => x.FullName)
            .Select(x => new PlayerStatsReadModel
            {
                PlayerId = x.Id,
                FullName = x.FullName,
                TournamentsCount = x.TeamPlayers.Count,
                FirstCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 1),
                SecondCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 2),
                ThirdCount = x.TeamPlayers.Count(x => x.Tournament.Status == Status.Finished && x.Team.Position == 3)
            })
            .ToListAsync(ct);
    }
}
