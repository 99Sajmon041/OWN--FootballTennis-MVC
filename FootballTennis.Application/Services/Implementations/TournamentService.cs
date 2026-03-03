using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using FootballTennis.Shared.Enums;
using FootballTennis.Shared.Pagination;
using Microsoft.Extensions.Logging;

namespace FootballTennis.Application.Services.Implementations;

public sealed class TournamentService(
    ILogger<TournamentService> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : ITournamentService
{

    public async Task<PagedResult<TournamentListItemViewModel>> GetAllTournamentsAsync(PagedRequest request, CancellationToken ct)
    {
        var pageRequest = request.Normalize();

        var (totalTournamentsCount, tournaments) = await unitOfWork.TournamentRepository.GetAllTournamentsAsync(pageRequest, ct);

        var tournamentsVm = mapper.Map<List<TournamentListItemViewModel>>(tournaments);

        logger.LogInformation("User retrieved a list of all tournaments. Total count: {TournamentsCount}", totalTournamentsCount);

        return new PagedResult<TournamentListItemViewModel>
        {
            Page = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            Search = pageRequest.Search,
            SortBy = pageRequest.SortBy,
            Desc = pageRequest.Desc,
            TotalCount = totalTournamentsCount,
            Items = tournamentsVm
        };
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
        tournament.Status = Status.Scheduled;

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
                else if (set.ScoreTeam2 > set.ScoreTeam1)
                    winTwo++;
                else
                    continue;
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

    public async Task GenerateMatchesForTournamentAsync(int tournamentId, CancellationToken ct)
    {
        var tournament = await unitOfWork.TournamentRepository.GetTournamentByIdAsync(tournamentId, ct);
        if (tournament is null)
        {
            logger.LogWarning("Tournament not found. Tournament ID: {TournamentId}.", tournamentId);
            throw new NotFoundException("Turnaj neexistuje.");
        }

        if (tournament.Status != Status.Scheduled)
        {
            logger.LogWarning("Cannot generate matches because tournament is not in Scheduled status. Tournament ID: {TournamentId}, Status: {Status}.",
                tournamentId,
                tournament.Status);

            throw new ConflictException("Zápasy lze vygenerovat pouze u turnaje, který není zahájen.");
        }

        if (tournament.Teams.Count < 3)
        {
            logger.LogWarning("Cannot generate matches because tournament has insufficient number of teams. Tournament ID: {TournamentId}, TeamsCount: {TeamsCount}.",
                tournamentId,
                tournament.Teams.Count);

            throw new ConflictException("Pro zahájení turnaje musí být v turnaji alespoň 3 týmy.");
        }

        var existsMatches = await unitOfWork.MatchRepository.ExistsAnyForTournamentAsync(tournamentId, ct);
        if (existsMatches)
        {
            logger.LogWarning("Cannot generate matches because matches already exist for tournament. Tournament ID: {TournamentId}.", tournamentId);
            throw new ConflictException("Zápasy už byly vygenerovány.");
        }

        var teams = tournament.Teams
            .OrderBy(x => x.Id)
            .Cast<Team?>()
            .ToList();

        if (teams.Count % 2 != 0)
        {
            teams.Add(null);
        }

        var teamsCount = teams.Count;
        var rounds = teamsCount - 1;
        var matchesPerRound = teamsCount / 2;
        var order = 1;

        for (int round = 0; round < rounds; round++)
        {
            for (int m = 0; m < matchesPerRound; m++)
            {
                var teamOne = teams[m];
                var teamTwo = teams[teamsCount - 1 - m];

                if (teamOne is null || teamTwo is null)
                {
                    continue;
                }

                var match = new Match
                {
                    TournamentId = tournament.Id,
                    TeamOneId = teamOne.Id,
                    TeamTwoId = teamTwo.Id,
                    Status = Status.InProgress,
                    Order = order,
                    Sets =
                    [
                        new Set
                        { 
                            SetNumber = 1,
                            ScoreTeam1 = 0,
                            ScoreTeam2 = 0,
                        },
                        new Set
                        { 
                            SetNumber = 2,
                            ScoreTeam1 = 0,
                            ScoreTeam2 = 0,
                        }
                    ]
                };

                tournament.Matches.Add(match);
                order++;
            }

            var last = teams[teamsCount - 1];
            for (int index = teamsCount - 1; index >= 2; index--)
            {
                teams[index] = teams[index - 1];
            }
            teams[1] = last;
        }

        tournament.Status = Status.InProgress;
        await unitOfWork.SaveChangesAsync(ct);
    }
}
