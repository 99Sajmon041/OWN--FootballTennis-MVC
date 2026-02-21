using AutoMapper;
using FootballTennis.Application.Models.Player;
using FootballTennis.Domain.Entities;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Application.MappingProfiles;

public sealed class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<PlayerStatsReadModel, PlayerStatsListItemViewModel>();

        CreateMap<UpsertPlayerViewModel, Player>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<Player, UpsertPlayerViewModel>();

        CreateMap<Player, PlayerViewModel>();
    }
}
