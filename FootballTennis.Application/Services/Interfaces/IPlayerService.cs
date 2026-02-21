using FootballTennis.Application.Models.Player;

namespace FootballTennis.Application.Services.Interfaces;

public interface IPlayerService
{
    Task<IReadOnlyList<PlayerStatsListItemViewModel>> GetPlayerStatsListAsync(CancellationToken ct);
    Task CreatePlayerAsync(UpsertPlayerViewModel createPlayerViewModel, CancellationToken ct);
    Task DeletePlayerAsync(int playerId, CancellationToken ct);
    Task<UpsertPlayerViewModel> GetPlayerForUpdateAsync(int playerId, CancellationToken ct);
    Task UpdatePlayerAsync(int playerId, UpsertPlayerViewModel model, CancellationToken ct);
}
