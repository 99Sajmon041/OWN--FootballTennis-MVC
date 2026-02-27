using FootballTennis.Application.Models.Team;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITeamService
{
    Task<List<PlayerOptionViewModel>> GetPlayersForTeamCreateAsync(int tournamentId, CancellationToken ct);
    Task AddTeamAsync(TeamUpsertViewModel model, CancellationToken ct);
    Task DeleteTeamAsync(int tournamentId, int id, CancellationToken ct);
    Task<TeamUpsertViewModel> GetTeamForUpdateAsync(int tournamentId, int id, CancellationToken ct);
    Task UpdateTeamAsync(int tournamentId, int id, TeamUpsertViewModel model, CancellationToken ct);
}
