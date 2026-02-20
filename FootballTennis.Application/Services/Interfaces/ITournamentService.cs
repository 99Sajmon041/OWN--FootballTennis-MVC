using FootballTennis.Application.Models.Tournament;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITournamentService
{
    Task<IReadOnlyList<TournamentListItemViewModel>> GetAllTournamentsAsync(CancellationToken ct);
}
