namespace FootballTennis.Domain.Interfaces;

public interface IUnitOfWork
{
    ITournamentRepository TournamentRepository { get; }
    IPlayerRepository PlayerRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
