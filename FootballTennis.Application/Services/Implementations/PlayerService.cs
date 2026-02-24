using AutoMapper;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Player;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FootballTennis.Application.Services.Implementations;

public sealed class PlayerService(
    ILogger<PlayerService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IPlayerService
{
    public async Task<IReadOnlyList<PlayerStatsListItemViewModel>> GetPlayerStatsListAsync(CancellationToken ct)
    {
        var playersStats = await unitOfWork.PlayerRepository.GetPlayersModelsStatsAsync(ct);

        var playersStatsModel = mapper.Map<List<PlayerStatsListItemViewModel>>(playersStats);

        logger.LogInformation("User retrieved a list of all players. Total count: {PlayersCount}", playersStats.Count);

        return playersStatsModel;
    }

    public async Task CreatePlayerAsync(UpsertPlayerViewModel createPlayerViewModel, CancellationToken ct)
    {
        var playerExists = await unitOfWork.PlayerRepository.ExistsPlayerWithSameNameAsync(createPlayerViewModel.FullName, ct);

        if (playerExists)
        {
            logger.LogError("Admin tries to create player with existing name. Name: {PlayerName}", createPlayerViewModel.FullName);
            throw new ConflictException($"Hráč se se jménem: {createPlayerViewModel.FullName} již existuje.");
        }

        var player = mapper.Map<Player>(createPlayerViewModel);

        await unitOfWork.PlayerRepository.AddPlayerAsync(player, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin create player with name: {PlayerName}", createPlayerViewModel.FullName);
    }

    public async Task DeletePlayerAsync(int playerId, CancellationToken ct)
    {
        var player = await unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId, ct);
        if (player is null)
        {
            logger.LogWarning("Player with ID: {PlayerId} was not found.", playerId);
            throw new NotFoundException($"Hráč s ID: {playerId} nebyl nalezen.");
        }

        unitOfWork.PlayerRepository.DeletePlayer(player);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Player with ID: {PlayerId} was deleted.", playerId);
    }

    public async Task<UpsertPlayerViewModel> GetPlayerForUpdateAsync(int playerId, CancellationToken ct)
    {
        var player = await unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId, ct);
        if (player is null)
        {
            logger.LogWarning("Player with ID: {PlayerId} was not found.", playerId);
            throw new NotFoundException($"Hráč s ID: {playerId} nebyl nalezen.");
        }

        return mapper.Map<UpsertPlayerViewModel>(player);
    }

    public async Task UpdatePlayerAsync(int playerId, UpsertPlayerViewModel model, CancellationToken ct)
    {
        model.FullName = model.FullName.Trim();

        var player = await unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId, ct);
        if (player is null)
        {
            logger.LogWarning("Player with ID: {PlayerId} was not found.", playerId);
            throw new NotFoundException($"Hráč s ID: {playerId} nebyl nalezen.");
        }

        var existsWithSameName = await unitOfWork.PlayerRepository.ExistsWithSameNameExceptId(playerId, model.FullName, ct);
        if (existsWithSameName)
        {
            logger.LogWarning("Player with same name already exists. Player ID: {PlayerId}, name: {PlayerName}.", playerId, model.FullName);
            throw new ConflictException("Již existuje hráč se stejným jménem.");
        }

        mapper.Map(model, player);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Player with ID: {PlayerId} was updated successfully.", playerId);
    }
}
