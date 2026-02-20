using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class UnitOfWork(FootballTennisDbContext context) : IUnitOfWork
{
    private ITournamentRepository? tournamentRepository;
    private IPlayerRepository? playerRepository;

    public ITournamentRepository TournamentRepository => tournamentRepository ??= new TournamentRepository(context);
    public IPlayerRepository PlayerRepository => playerRepository ??= new PlayerRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }  
}
