using FootballTennis.Application.Models.Team;

namespace FootballTennis.Application.Services.Interfaces;

public interface ITeamService
{
    Task<List<PlayerOptionViewModel>> GetPlayersForTeamCreateAsync(CancellationToken ct);
    Task AddTeamAsync(CreateTeamViewModel model, CancellationToken ct);
}
