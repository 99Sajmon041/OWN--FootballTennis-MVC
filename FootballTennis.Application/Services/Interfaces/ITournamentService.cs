using FootballTennis.Application.Models.Tournament;
using FootballTennis.Shared.Pagination;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITournamentService
{
    Task<PagedResult<TournamentListItemViewModel>> GetAllTournamentsAsync(PagedRequest request, CancellationToken ct);
    Task CreateTournamentAsync(TournamentUpsertViewModel model, CancellationToken ct);
    Task UpdateTournamentAsync(int tournamentId, TournamentUpsertViewModel model, CancellationToken ct);
    Task<TournamentUpsertViewModel> GetTournamentForUpdateAsync(int tournamentId, CancellationToken ct);
    Task DeleteTournamentAsync(int tournamentId, CancellationToken ct);
    Task<TournamentDetailViewModel> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct);
    Task GenerateMatchesForTournamentAsync(int tournamentId, CancellationToken ct);
    Task EvaluateTournamentAsync(int tournamentId, CancellationToken ct);
    Task<TournamentStatisticsViewModel> GetTournamentStatisticsAsync(int tournamentId, CancellationToken ct);
    Task<TournamentsTeamStatisticsViewModel> GetTournamentsTeamStatisticsAsync(int teamId, int tournamentId, CancellationToken ct);
} 