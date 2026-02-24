using AutoMapper;
using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Models.Player;
using FootballTennis.Application.Models.Team;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Domain.Entities;
using FootballTennis.Domain.Enums;

namespace FootballTennis.Application.MappingProfiles;

public sealed class TournamentMappingProfile : Profile
{
    public TournamentMappingProfile()
    {
        CreateMap<Tournament, TournamentListItemViewModel>()
            .ForMember(x => x.TeamsCount, opt => opt.MapFrom(x => x.Teams.Count))
            .ForMember(x => x.MatchesCount, opt => opt.MapFrom(x => x.Matches.Count))
            .ForMember(x => x.WinnerName, opt => opt.MapFrom(x => x.Status == Status.Finished ?
                x.Teams.Where(x => x.Position == 1).Select(x => x.Name).FirstOrDefault() : null));

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
