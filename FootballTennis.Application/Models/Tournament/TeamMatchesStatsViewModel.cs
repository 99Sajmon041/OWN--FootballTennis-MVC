namespace FootballTennis.Application.Models.Tournament;

public sealed class TeamMatchesStatsViewModel
{
    public string TeamName { get; set; } = default!;
    public List<TeamSetsStatsViewModel> Sets { get; set; } = [];
}
