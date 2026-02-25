using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Team;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Enums;
using FootballTennis.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FootballTennis.Application.Services.Implementations;

public sealed class TeamService(
    ILogger<TeamService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ITeamService
{
    public async Task<List<PlayerOptionViewModel>> GetPlayersForTeamCreateAsync(int tournamentId, CancellationToken ct)
    {
        var players = await unitOfWork.PlayerRepository.GetAllPlayersAsync(ct);

        var assignedIds = (await unitOfWork.TeamPlayerRepository
            .GetAssignedPlayerIdsAsync(tournamentId, ct))
            .ToHashSet();

        var playersVm = mapper.Map<List<PlayerOptionViewModel>>(players);

        foreach (var player in playersVm)
        {
            player.IsAlreadyAssigned = assignedIds.Contains(player.Id);
        }

        logger.LogInformation("Admin fetched {PlayersCount} players to drop-down.", players.Count);

        return playersVm;
    }

    public async Task AddTeamAsync(CreateTeamViewModel model, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var playersIdDistinct = model.SelectedPlayersId
            .Distinct()
            .ToList();

        if (playersIdDistinct.Count != model.TeamPlayersCount)
        {
            logger.LogError("Team players count does not match. TournamentId: {TournamentId}.", model.TournamentId);
            throw new ConflictException("Počet hráčů neodpovídá.");
        }

        var hasTournamentStatusScheduled = await unitOfWork.TournamentRepository.IsTournamentStatusScheduled(model.TournamentId, ct);

        if (!hasTournamentStatusScheduled)
        {
            logger.LogError("Tournament is not scheduled. Not possible to add team. TournamentId: {TournamentId}.", model.TournamentId);
            throw new DomainException("Turnaj již není ve stavu, kdy je možné upravovat.");
        }

        var existsPlayersAtTournament = await unitOfWork.TeamPlayerRepository.AlreadyExistsPlayersAtTournamentAsync(model.TournamentId, playersIdDistinct, ct);

        if (existsPlayersAtTournament)
        {
            logger.LogError("Players already exist in another team. TournamentId: {TournamentId}.", model.TournamentId);
            throw new ConflictException("Hráči v týmu již existují v jiném týmu.");
        }

        var team = mapper.Map<Team>(model);

        team.TeamPlayers = playersIdDistinct.Select(x => new TeamPlayer
        {
            TournamentId = model.TournamentId,
            PlayerId = x
        })
        .ToList();

        unitOfWork.TeamRepository.AddTeam(team);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin created team with name: {TeamName}.", model.Name);
    }

    public async Task DeleteTeamAsync(int tournamentId, int id, CancellationToken ct)
    {
        var team = await unitOfWork.TeamRepository.GetTeamByIdAsync(id, ct);
        if (team is null)
        {
            logger.LogInformation("Team does not exists. Team ID: {TeamId}.", id);
            throw new ConflictException("Tým nebyl nalezen.");
        }

        var istournamentStatusScheduled = await unitOfWork.TournamentRepository.IsTournamentStatusScheduled(tournamentId, ct);
        if (!istournamentStatusScheduled)
        {
            logger.LogInformation("Admin tries to delete Team, bud tournament has not set status to scheduled. Tournament ID: {TournamentId}.", tournamentId);
            throw new DomainException("Tým nejde smazat, turnaj není ve stavu, kdy je možný upravit.");
        }

        if (tournamentId != team.TournamentId)
        {
            logger.LogInformation("Team in tournament does not exists. Team ID: {TeamId}, tournament ID: {TournamentId}.", id, tournamentId);
            throw new ConflictException("Tým v rámci tohoto turnaje neexistuje.");
        }

        unitOfWork.TeamRepository.DeleteTeam(team);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Team with ID: {TeamId} was deleted from tournament with ID: {TournamentId}.", id, tournamentId);
    }
}