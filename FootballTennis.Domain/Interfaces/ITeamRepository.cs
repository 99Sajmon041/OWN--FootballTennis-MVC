using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface ITeamRepository
{
    void AddTeamAsync(Team team);
}
