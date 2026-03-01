using FootballTennis.Application.Models.Player;
using FootballTennis.Shared.Pagination;

namespace FootballTennis.Application.Services.Interfaces;

public interface IPlayerService
{
    Task<PagedResult<PlayerStatsListItemViewModel>> GetPlayerStatsListAsync(PagedRequest request, CancellationToken ct);
    Task CreatePlayerAsync(UpsertPlayerViewModel createPlayerViewModel, CancellationToken ct);
    Task DeletePlayerAsync(int playerId, CancellationToken ct);
    Task<UpsertPlayerViewModel> GetPlayerForUpdateAsync(int playerId, CancellationToken ct);
    Task UpdatePlayerAsync(int playerId, UpsertPlayerViewModel model, CancellationToken ct);
}
