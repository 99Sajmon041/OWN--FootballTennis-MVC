using FootballTennis.Domain.Entities;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerStatsReadModel>> GetPlayersModelsStatsAsync(CancellationToken ct);
    Task AddPlayerAsync(Player player, CancellationToken ct);
    Task<bool> ExistsPlayerWithSameNameAsync(string fullName, CancellationToken ct);
    Task<bool> ExistsWithSameNameExceptId(int playerId, string playerName, CancellationToken ct);
    void DeletePlayer(Player player);
    Task<Player?> GetPlayerByIdAsync(int playerId, CancellationToken ct);
    Task<List<Player>> GetAllPlayersAsync(CancellationToken ct);
}
