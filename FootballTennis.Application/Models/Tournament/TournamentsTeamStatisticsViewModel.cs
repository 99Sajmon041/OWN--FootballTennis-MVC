using FootballTennis.Application.Models.Player;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentsTeamStatisticsViewModel
{
    public int TournamentId { get; set; }
    public string TournamentName { get; set; } = default!;
    public string TeamName { get; set; } = default!;
    public int Position { get; set; }
    public int WonMatches { get; set; }
    public int LostMatches { get; set; }
    public List<TeamMatchesStatsViewModel> Matches { get; set; } = [];
    public List<SelectListItem> Teams { get; set; } = [];
    public List<PlayerViewModel> Players { get; set; } = [];
    public int TeamId { get; set; }
}
