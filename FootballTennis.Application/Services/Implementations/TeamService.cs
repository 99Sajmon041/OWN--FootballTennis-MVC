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
    public async Task<List<PlayerOptionViewModel>> GetPlayersForTeamCreateAsync(CancellationToken ct)
    {
        var players = await unitOfWork.PlayerRepository.GetAllPlayersAsync(ct);

        var playersVm = mapper.Map<List<PlayerOptionViewModel>>(players);

        logger.LogInformation("Admin fetched {PlayersCount} players to drop-down.", players.Count);

        return playersVm;
    }

    public async Task AddTeamAsync(CreateTeamViewModel model, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        /*
         🔎 Ještě jedna věc (architektonicky důležitá)

            Před uložením bys měl validovat:

            SelectedPlayersId.Count == model.TeamPlayersCount

            žádné duplicity (Distinct)

            hráč už není v jiném týmu v tom turnaji

            turnaj je Scheduled

            Tohle je důležité kvůli integritě.

            https://chatgpt.com/g/g-p-699338afa7e481918c69b64b9783328a-footbal-tennis/c/699c7dba-4f74-8388-8994-b285c02c1f8d
         */

        if (model.SelectedPlayersId.Count != model.TeamPlayersCount)
        {
            logger.LogError("");
            throw new ConflictException("");
        }

        var team = mapper.Map<Team>(model);

        team.TeamPlayers = model.SelectedPlayersId.Select(x => new TeamPlayer
        {
            TournamentId = model.TournamentId,
            PlayerId = x,
        })
        .ToList();


        unitOfWork.TeamRepository.AddTeamAsync(team);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin created team with name: {TeamName}.", model.Name);
    }
}