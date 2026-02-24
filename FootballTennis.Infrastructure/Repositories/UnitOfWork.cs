using FootballTennis.Domain.Interfaces;
using FootballTennis.Infrastructure.Database;

namespace FootballTennis.Infrastructure.Repositories;

public sealed class UnitOfWork(FootballTennisDbContext context) : IUnitOfWork
{
    private ITournamentRepository? tournamentRepository;
    private IPlayerRepository? playerRepository;
    private ITeamRepository? teamRepository;

    public ITournamentRepository TournamentRepository => tournamentRepository ??= new TournamentRepository(context);
    public IPlayerRepository PlayerRepository => playerRepository ??= new PlayerRepository(context);
    public ITeamRepository TeamRepository => teamRepository ??= new TeamRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return context.SaveChangesAsync(ct);
    }  
}
