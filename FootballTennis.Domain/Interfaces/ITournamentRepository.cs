using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface ITournamentRepository
{
    Task<IReadOnlyList<Tournament>> GetAllTournamentsAsync(CancellationToken ct);
    void AddTournament(Tournament tournament);
    void DeleteTournament(Tournament tournament);
    Task<bool> ExistsTournamentWithSameName(string tournamentName, CancellationToken ct);
    Task<bool> ExistsWithSameNameExceptId(int tournamentId, string tournamentName, CancellationToken ct);
    Task<Tournament?> GetTournamentByIdAsync(int tournamentId, CancellationToken ct);
    Task<Tournament?> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct);
}
