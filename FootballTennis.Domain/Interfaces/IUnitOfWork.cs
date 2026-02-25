namespace FootballTennis.Domain.Interfaces;

public interface IUnitOfWork
{
    ITournamentRepository TournamentRepository { get; }
    IPlayerRepository PlayerRepository { get; }
    ITeamRepository TeamRepository { get; }
    ITeamPlayerRepository TeamPlayerRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
