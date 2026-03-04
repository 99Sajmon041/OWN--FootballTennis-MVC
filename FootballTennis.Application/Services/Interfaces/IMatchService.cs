using FootballTennis.Application.Models.Match;

namespace FootballTennis.Application.Services.Interfaces;

public interface IMatchService
{
    Task<MatchUpdateViewModel> GetMatchForUpdateAsync(int id, int tournamentId, CancellationToken ct);
    Task UpdateMatchAsync(int id, int tournamentId, MatchUpdateViewModel model, CancellationToken ct);
}
