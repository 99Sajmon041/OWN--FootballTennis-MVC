using AutoMapper;
using FootballTennis.Application.Models.Team;
using FootballTennis.Domain.Entities;

namespace FootballTennis.Application.MappingProfiles;

public sealed class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<CreateTeamViewModel, Team>();
    }
}
