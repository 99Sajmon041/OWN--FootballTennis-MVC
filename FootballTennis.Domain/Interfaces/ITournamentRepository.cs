using FootballTennis.Domain.Entities;

namespace FootballTennis.Domain.Interfaces;

public interface ITournamentRepository
{
    Task<IReadOnlyList<Tournament>> GetAllTournamentsAsync(CancellationToken ct);
}
