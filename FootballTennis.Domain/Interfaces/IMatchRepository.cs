using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface IMatchRepository
{
    Task<bool> ExistsAnyForTournamentAsync(int tournamentId, CancellationToken ct);
    Task<Match?> GetMatchForUpdateAsync(int id, int tournamentId, CancellationToken ct);
}
