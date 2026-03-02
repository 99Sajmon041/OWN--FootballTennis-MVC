namespace FootballTennis.Domain.Interfaces;

public interface IMatchRepository
{
    Task<bool> ExistsAnyForTournamentAsync(int tournamentId, CancellationToken ct);
}
