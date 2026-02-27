using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Team;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Entities;
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

    public async Task AddTeamAsync(TeamUpsertViewModel model, CancellationToken ct)
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

    public async Task<TeamUpsertViewModel> GetTeamForUpdateAsync(int tournamentId, int id, CancellationToken ct)
    {
        var team = await unitOfWork.TeamRepository.GetTeamByIdWithDetailsAsync(tournamentId, id, ct);
        if (team is null)
        {
            logger.LogInformation("Team in tournament does not exists. Team ID: {TeamId}, tournament ID: {TournamentId}.", id, tournamentId);
            throw new NotFoundException("Tým neexistuje.");
        }

        var istournamentStatusScheduled = await unitOfWork.TournamentRepository.IsTournamentStatusScheduled(tournamentId, ct);
        if (!istournamentStatusScheduled)
        {
            logger.LogInformation("Admin tries to update Team, bud tournament has not set status to scheduled. Tournament ID: {TournamentId}.", tournamentId);
            throw new DomainException("Tým nejde upravit, turnaj není ve stavu, kdy je možný upravit.");
        }

        var updateModel = new TeamUpsertViewModel
        {
            Id = team.Id,
            TournamentId = team.TournamentId,
            Name = team.Name,
            TeamPlayersCount = team.TeamPlayers.Count,
            Players = await GetPlayersForTeamCreateAsync(tournamentId, ct),
            SelectedPlayersId = team.TeamPlayers.Select(x => x.PlayerId).ToList()
        };

        foreach (var player in updateModel.Players)
        {
            if (updateModel.SelectedPlayersId.Contains(player.Id))
            {
                player.IsAlreadyAssigned = false;
            }
        }

        logger.LogInformation("Admin retrieved team model for update. Tournament ID: {TournamentId}, Team ID: {TeamId}.", team.TournamentId, team.Id);

        return updateModel;
    }

    public async Task UpdateTeamAsync(int tournamentId, int id, TeamUpsertViewModel model, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (model.TournamentId != tournamentId)
        {
            logger.LogWarning("UpdateTeam: TournamentId mismatch. Route TournamentId: {RouteTournamentId}, Model TournamentId: {ModelTournamentId}.", 
                tournamentId,
                model.TournamentId);

            throw new DomainException("Neplatný požadavek.");
        }

        var team = await unitOfWork.TeamRepository.GetTeamByIdWithDetailsAsync(tournamentId, id, ct);
        if (team is null)
        {
            logger.LogInformation("Team in tournament does not exist. TeamId: {TeamId}, TournamentId: {TournamentId}.", id, tournamentId);

            throw new NotFoundException("Tým neexistuje.");
        }

        var isTournamentStatusScheduled = await unitOfWork.TournamentRepository.IsTournamentStatusScheduled(tournamentId, ct);
        if (!isTournamentStatusScheduled)
        {
            logger.LogInformation("Admin tries to update team, but tournament is not scheduled. TournamentId: {TournamentId}.", tournamentId);

            throw new DomainException("Tým nejde upravit, turnaj není ve stavu, kdy je možné upravovat.");
        }

        var playersIdDistinct = model.SelectedPlayersId
            .Distinct()
            .ToList();

        if (playersIdDistinct.Count != model.TeamPlayersCount)
        {
            logger.LogWarning(
                "Team players count does not match. TournamentId: {TournamentId}, Expected: {ExpectedCount}, ReceivedDistinct: {ReceivedCount}.",
                tournamentId,
                model.TeamPlayersCount,
                playersIdDistinct.Count);

            throw new ConflictException("Počet hráčů neodpovídá.");
        }

        var existsPlayersInOtherTeam = await unitOfWork.TeamPlayerRepository.AlreadyExistsPlayersInOtherTeamAsync(tournamentId, team.Id, playersIdDistinct, ct);

        if (existsPlayersInOtherTeam)
        {
            logger.LogWarning("Players already exist in another team. TournamentId: {TournamentId}, TeamId: {TeamId}.", tournamentId, team.Id);

            throw new ConflictException("Hráči v týmu již existují v jiném týmu.");
        }

        team.Name = model.Name.Trim();

        team.TeamPlayers.Clear();

        foreach (var playerId in playersIdDistinct)
        {
            team.TeamPlayers.Add(new TeamPlayer
            {
                TournamentId = tournamentId,
                PlayerId = playerId
            });
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation( "Team updated successfully. TournamentId: {TournamentId}, TeamId: {TeamId}.", tournamentId, team.Id);
    }
} 