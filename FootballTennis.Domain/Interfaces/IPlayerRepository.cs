using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerStatsReadModel>> GetPlayersModelsStatsAsync(CancellationToken ct);
}
