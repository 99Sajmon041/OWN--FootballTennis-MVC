using FootballTennis.Application.Models.Tournament;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITournamentService
{
    Task<IReadOnlyList<TournamentListItemViewModel>> GetAllTournamentsAsync(CancellationToken ct);
    Task CreateTournamentAsync(TournamentUpsertViewModel model, CancellationToken ct);
    Task UpdateTournamentAsync(int tournamentId, TournamentUpsertViewModel model, CancellationToken ct);
    Task<TournamentUpsertViewModel> GetTournamentForUpdateAsync(int tournamentId, CancellationToken ct);
    Task DeleteTournamentAsync(int tournamentId, CancellationToken ct);
    Task<TournamentDetailViewModel> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct);
}
