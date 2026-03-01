using AutoMapper;
using FootballTennis.Application.Models.Player;
using FootballTennis.Application.Models.Team;
using FootballTennis.Domain.Entities;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Application.MappingProfiles;

public sealed class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<PlayerListItemReadModel, PlayerStatsListItemViewModel>();

        CreateMap<UpsertPlayerViewModel, Player>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<Player, UpsertPlayerViewModel>();

        CreateMap<Player, PlayerViewModel>();

        CreateMap<Player, PlayerOptionViewModel>();
    }
}
