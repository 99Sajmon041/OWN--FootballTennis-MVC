using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Entities;
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


    public async Task CreateTournamentAsync(TournamentUpsertViewModel model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();

        var existsTournamentWithSameName = await unitOfWork.TournamentRepository.ExistsTournamentWithSameName(model.Name, ct);
        if(existsTournamentWithSameName)
        {
            logger.LogWarning("Admin tries to create tournament with name that already exists. Tournament name: {TournamentName}.", model.Name);
            throw new ConflictException("Turnaj se stejným názvem již existuje - změňte název.");
        }

        var tournament = mapper.Map<Tournament>(model);
        tournament.Status = Domain.Enums.Status.Scheduled;

        unitOfWork.TournamentRepository.AddTournament(tournament);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin created tournament. Tournament name: {TournamentName}, address: {TournamentAddress}.",
            tournament.Name,
            tournament.Address);
    }

    public async Task UpdateTournamentAsync(int tournamentId, TournamentUpsertViewModel model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();

        var tournament = await unitOfWork.TournamentRepository.GetTournamentByIdAsync(tournamentId, ct);
        if (tournament is null)
        {
            logger.LogWarning("Tournament was not found. Tournament ID: {TournamentId}.", tournamentId);
            throw new NotFoundException("Turnaj nebyl nalezen.");
        }

        if (tournament.Teams.Count > 0)
        {
            logger.LogWarning("Tournament with at least one team is not possible to edit. Tournament ID: {TournamentId}, name: {TournamentName}.", tournamentId, model.Name);
            throw new ConflictException("Turnaj s alespoň jedním týmem již není možné upravovat.");
        }

        var existsWithSameName = await unitOfWork.TournamentRepository.ExistsWithSameNameExceptId(tournamentId, model.Name, ct);
        if (existsWithSameName)
        {
            logger.LogWarning("Tournament with same name already exists. Tournament ID: {TournamentId}, name: {TournamentName}.",tournamentId, model.Name); 
            throw new ConflictException("Již existuje Turnaj se stejným názvem.");
        }

        mapper.Map(model, tournament);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Tournament was updated successfully. Tournament ID: {TournamentId}.", tournamentId);
    }

    public async Task<TournamentUpsertViewModel> GetTournamentForUpdateAsync(int tournamentId, CancellationToken ct)
    {
        var tournament = await unitOfWork.TournamentRepository.GetTournamentByIdAsync(tournamentId, ct);
        if (tournament is null)
        {
            logger.LogWarning("Tournament was not found. Tournament ID: {TournamentId}.", tournamentId);
            throw new NotFoundException("Turnaj nebyl nalezen.");
        }

        logger.LogInformation("Admin retrieved tournament with ID: {TournamentId}.", tournamentId);

        return mapper.Map<TournamentUpsertViewModel>(tournament);
    }

    public async Task DeleteTournamentAsync(int tournamentId, CancellationToken ct)
    {
        var tournament = await unitOfWork.TournamentRepository.GetTournamentByIdAsync(tournamentId, ct);
        if (tournament is null)
        {
            logger.LogWarning("Tournament with ID: {TournamentId} was not found.", tournamentId);
            throw new NotFoundException($"Turnaj s ID: {tournamentId} nebyl nalezen.");
        }

        unitOfWork.TournamentRepository.DeleteTournament(tournament);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Tournament with ID: {TournamentId} was deleted.", tournamentId);
    }

    public async Task<TournamentDetailViewModel> GetTournamentWithDetailsAsync(int tournamentId, CancellationToken ct)
    {
        var tournament = await unitOfWork.TournamentRepository.GetTournamentWithDetailsAsync(tournamentId, ct);
        if(tournament is null)
        {
            logger.LogWarning("Tournament with ID: {TournamentId} does not exists.", tournamentId);
            throw new NotFoundException($"Turnaj s ID: {tournamentId} neexistuje.");
        }

        var model = mapper.Map<TournamentDetailViewModel>(tournament);

        foreach (var match in tournament.Matches)
        {
            var winOne = 0;
            var winTwo = 0;

            foreach (var set in match.Sets)
            {
                if (set.ScoreTeam1 > set.ScoreTeam2)
                    winOne++;
                else
                    winTwo++;
            }

            var vmMatch = model.Matches.First(x => x.Id == match.Id);

            if (winOne == 0 && winTwo == 0)
            {
                vmMatch.ScoreText = "-";
                vmMatch.IsPlayed = false;
            }
            else
            {
                vmMatch.ScoreText = $"{winOne} : {winTwo}";
                vmMatch.IsPlayed = true;
            }
        }

        return model;
    }
}
