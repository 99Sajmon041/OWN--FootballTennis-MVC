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

        var orderedSets = model.Sets
            .OrderBy(x => x.SetNumber)
            .ToList();

        int teamOneWins = 0;
        int teamTwoWins = 0;

        for (int i = 0; i < orderedSets.Count; i++)
        {
            var set = orderedSets[i];

            bool bothNull = set.ScoreTeam1 is null && set.ScoreTeam2 is null;
            bool oneNull = set.ScoreTeam1 is null || set.ScoreTeam2 is null;

            if (!bothNull && set.ScoreTeam1!.Value == set.ScoreTeam2!.Value)
            {
                throw new ConflictException("Nelze nastavit stejné skóre pro oba týmy.");
            }

            if (bothNull)
            {
                bool hasFilledAfter = orderedSets
                    .Skip(i + 1)
                    .Any(s => s.ScoreTeam1 is not null || s.ScoreTeam2 is not null);
            
                if (hasFilledAfter)
                {
                    logger.LogWarning("Admin tries to set sets out of order. Match ID: {MatchId}.", model.Id);
                    throw new ConflictException("Nelze nastavovat sety na přeskáčku.");
                }

                for (int j = i + 1; j < orderedSets.Count; j++)
                {
                    orderedSets[j].ScoreTeam1 = null;
                    orderedSets[j].ScoreTeam2 = null;
                }

                break;
            }


            if (oneNull)
            {
                logger.LogWarning("Set has only one score filled. Match ID: {MatchId}, SetNumber: {SetNumber}.", model.Id, set.SetNumber);
                throw new ConflictException("Set musí mít vyplněné skóre pro oba týmy, nebo pro oba prázdné.");
            }

            if (set.ScoreTeam1 < 0 || set.ScoreTeam1 > 25 || set.ScoreTeam2 < 0 || set.ScoreTeam2 > 25)
            {
                logger.LogWarning("Invalid set score range (0-25). Match ID: {MatchId}, SetNumber: {SetNumber}.", model.Id, set.SetNumber);
                throw new ConflictException("Skóre setu musí být v rozmezí 0–25.");
            }

            int s1 = set.ScoreTeam1!.Value;
            int s2 = set.ScoreTeam2!.Value;

            bool isFinished = ((Math.Max(s1, s2) == 11) && (Math.Abs(s1 - s2) >= 2) || (Math.Max(s1, s2) > 11) && (Math.Abs(s1 - s2) == 2));

            if (!isFinished)
            {
                logger.LogWarning("Invalid finished set score. Match ID: {MatchId}, SetNumber: {SetNumber}, Score: {S1}:{S2}.", model.Id, set.SetNumber, s1, s2);
                throw new ConflictException($"Nezadali jste platný finální výsledek setu číslo: {set.SetNumber}.");
            }

            if (s1 > s2)
            {
                teamOneWins++;
            }
            else
            {
                teamTwoWins++;
            }

            if (teamOneWins == 2 || teamTwoWins == 2)
            {
                for (int j = i + 1; j < orderedSets.Count; j++)
                {
                    orderedSets[j].ScoreTeam1 = null;
                    orderedSets[j].ScoreTeam2 = null;
                }

                break;
            }
        }

        if (teamOneWins > 2 || teamTwoWins > 2 || (teamOneWins + teamTwoWins) > 3)
        {
            logger.LogWarning("Match with ID: {MatchId} has no valid score - unable to save.", model.Id);
            throw new ConflictException("Zápas má neplatné skóre - nelze uložit.");
        }

        var modelSetsById = orderedSets.ToDictionary(x => x.Id);

        foreach (var setEntity in match.Sets)
        {
            if (modelSetsById.TryGetValue(setEntity.Id, out var setVm))
            {
                setEntity.ScoreTeam1 = setVm.ScoreTeam1;
                setEntity.ScoreTeam2 = setVm.ScoreTeam2;
            }
        }

        match.PlayedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("The score of match with ID: {MatchId} was updated successfully.", model.Id);
    }
}
