using AutoMapper;
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
    }
}
