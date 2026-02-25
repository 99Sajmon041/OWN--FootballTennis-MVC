using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface ITeamPlayerRepository
{
    Task<bool> AlreadyExistsPlayersAtTournamentAsync(int tournamentId, List<int> playersId, CancellationToken ct);
    Task<List<int>> GetAssignedPlayerIdsAsync(int tournamentId, CancellationToken ct);
}
