using AutoMapper;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FootballTennis.Application.Services.Implementations;

public sealed class TournamentService(
    ILogger<TournamentService> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : ITournamentService
{
    public async Task<IReadOnlyList<TournamentListItemViewModel>> GetAllTournamentsAsync(CancellationToken ct)
    {
        var tournaments = await unitOfWork.TournamentRepository.GetAllTournamentsAsync(ct);

        var tournamentsModels = mapper.Map<List<TournamentListItemViewModel>>(tournaments);

        logger.LogInformation("User retrieved a list of all tournaments. Total count: {TournamentsCount}", tournaments.Count);

        return tournamentsModels;
    }
}
