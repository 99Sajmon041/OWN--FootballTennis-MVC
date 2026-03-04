using AutoMapper;
using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Models.Set;
using FootballTennis.Domain.Entities;

namespace FootballTennis.Application.MappingProfiles;

public sealed class MatchMappingProfile : Profile
{
    public MatchMappingProfile()
    {
        CreateMap<Set, SetUpdateListItemViewModel>();

        CreateMap<Match, MatchUpdateViewModel>()
            .ForMember(x => x.TeamOneName, opt => opt.MapFrom(x => x.TeamOne.Name))
            .ForMember(x => x.TeamTwoName, opt => opt.MapFrom(x => x.TeamTwo.Name))
            .ForMember(x => x.Sets, opt => opt.MapFrom(x => x.Sets));
    }
}
