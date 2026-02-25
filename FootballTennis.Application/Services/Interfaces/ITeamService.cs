using FootballTennis.Application.Models.Team;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITeamService
{
    Task<List<PlayerOptionViewModel>> GetPlayersForTeamCreateAsync(int tournamentId, CancellationToken ct);
    Task AddTeamAsync(CreateTeamViewModel model, CancellationToken ct);
    Task DeleteTeamAsync(int tournamentId, int id, CancellationToken ct);
}
