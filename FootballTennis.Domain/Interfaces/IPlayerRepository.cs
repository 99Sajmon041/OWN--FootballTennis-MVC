using FootballTennis.Domain.Entities;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerStatsReadModel>> GetPlayersModelsStatsAsync(CancellationToken ct);
    Task AddPlayerAsync(Player player, CancellationToken ct);
    Task<bool> ExistsPlayerWithSameNameAsync(string fullName, CancellationToken ct);
    void DeletePlayer(Player player, CancellationToken ct);
    Task<Player?> GetPlayerByIdAsync(int playerId, CancellationToken ct);
}
