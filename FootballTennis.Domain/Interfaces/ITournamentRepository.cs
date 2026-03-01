using FootballTennis.Domain.Entities;
using FootballTennis.Shared.Pagination;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Domain.Interfaces;

public interface ITournamentRepository
{
    Task<(int, IReadOnlyList<TournamentListItemReadModel>)> GetAllTournamentsAsync(PagedRequest request, CancellationToken ct);
    void AddTournament(Tournament tournament);
    void DeleteTournament(Tournament tournament);
    Task<bool> ExistsTournamentWithSameName(string tournamentName, CancellationToken ct);
    Task<bool> ExistsWithSameNameExceptId(int tournamentId, string tournamentName, CancellationToken ct);
    Task<Tournament?> GetTournamentByIdAsync(int tournamentId, CancellationToken ct);
    Task<Tournament?> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct);
    Task<bool> IsTournamentStatusScheduled(int tournamentId, CancellationToken ct);
}
