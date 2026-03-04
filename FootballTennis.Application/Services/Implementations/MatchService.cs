using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FootballTennis.Application.Services.Implementations;

public class MatchService(
    ILogger<MatchService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IMatchService
{
    public async Task<MatchUpdateViewModel> GetMatchForUpdateAsync(int id, int tournamentId, CancellationToken ct)
    {
        var match = await unitOfWork.MatchRepository.GetMatchForUpdateAsync(id, tournamentId, ct);
        if (match == null)
        {
            logger.LogWarning("Match with ID: {MatchId} was not found.", id);
            throw new NotFoundException("Zápas nebyl nalezen.");
        }

        logger.LogInformation("Match loaded for update. Match ID: {MatchId}.", id);

        return mapper.Map<MatchUpdateViewModel>(match);
    }

    public async Task UpdateMatchAsync(int id, int tournamentId, MatchUpdateViewModel model, CancellationToken ct)
    {
        if (id != model.Id || tournamentId != model.TournamentId)
        {
            logger.LogWarning("Parameters from URL are not same as the input parameters. Unable to udate Match with ID: {MatchId}.", model.Id);
            throw new DomainException("Vyplněné parametry nejsou shodné s parametry z URL, použijte formulář !");
        }

        var match = await unitOfWork.MatchRepository.GetMatchForUpdateAsync(model.Id, model.TournamentId, ct);
        if (match == null)
        {
            logger.LogWarning("Match with ID: {MatchId} was not found.", model.Id);
            throw new NotFoundException("Zápas nebyl nalezen.");
        }

        int teamOneWins = 0;
        int teamTwoWins = 0;

        foreach (var set in model.Sets)
        {
            if (set.ScoreTeam1 is null || set.ScoreTeam2 is null)
                continue;

            if ((0 > set.ScoreTeam1 || set.ScoreTeam1 > 25) || (0 > set.ScoreTeam2 || set.ScoreTeam2 > 25))
            {
                logger.LogWarning("Invalid set score range (0-25). Match ID: {MatchId}.", model.Id);
                throw new ConflictException("Skóre setu musí být v rozmezí 0–25.");
            }

            if (set.ScoreTeam1 > set.ScoreTeam2)
                teamOneWins++;
            else if (set.ScoreTeam1 < set.ScoreTeam2)
                teamTwoWins++;
            else
                continue;
        }

        if (teamOneWins > 2 || teamTwoWins > 2 || teamOneWins + teamTwoWins > 3)
        {
            logger.LogWarning("Match with ID: {MatchId} has no valid score - unable to save.", model.Id);
            throw new ConflictException("Zápas má neplatné skóre - nelze uložit.");
        }

        var modelSetsById = model.Sets.ToDictionary(x => x.Id);

        foreach (var setEntity in match.Sets)
        {
            if (modelSetsById.TryGetValue(setEntity.Id, out var setVm))
            {
                setEntity.ScoreTeam1 = setVm.ScoreTeam1;
                setEntity.ScoreTeam2 = setVm.ScoreTeam2;
            }
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("The score of match with ID: {MatchId} was updated successfully.", model.Id);
    }
}
