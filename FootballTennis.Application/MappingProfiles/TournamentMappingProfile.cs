using AutoMapper;
using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Models.Player;
using FootballTennis.Application.Models.Team;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Domain.Entities;
using FootballTennis.Shared.ReadModels;

namespace FootballTennis.Application.MappingProfiles;

public sealed class TournamentMappingProfile : Profile
{
    public TournamentMappingProfile()
    {
        CreateMap<TournamentListItemReadModel, TournamentListItemViewModel>();

        CreateMap<TournamentUpsertViewModel, Tournament>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<Tournament, TournamentUpsertViewModel>();

        CreateMap<Tournament, TournamentDetailViewModel>();

        CreateMap<Team, TeamListItemViewModel>();

        CreateMap<TeamPlayer, PlayerViewModel>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.Player.FullName));

        CreateMap<Match, MatchListItemViewModel>()
            .ForMember(x => x.TeamOneName, opt => opt.MapFrom(x => x.TeamOne.Name))
            .ForMember(x => x.TeamTwoName, opt => opt.MapFrom(x => x.TeamTwo.Name));
    }
}
