using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface ITeamRepository
{
    void AddTeam(Team team);
    void DeleteTeam(Team team);
    Task<Team?> GetTeamByIdAsync(int id, CancellationToken ct);
    Task<Team?> GetTeamByIdWithDetailsAsync(int tournamentId, int id, CancellationToken ct);
}
