using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentStatisticsListItemViewModel
{
    [Display(Name = "ID týmu")]
    public int TeamId { get; set; }

    [Display(Name = "Umístění")]
    public int Position { get; set; }

    [Display(Name = "Tým")]
    public string TeamName { get; set; } = default!;
    public int MatchesPlayed { get; set; }

    [Display(Name = "Výhry")]
    public int WonMatches { get; set; }

    [Display(Name = "Prohry")]
    public int LostMatches => MatchesPlayed > 0 ? MatchesPlayed - WonMatches : 0;
    public int SetsPlayed { get; set; }

    [Display(Name = "Výhry (sety)")]
    public int WonSets { get; set; }

    [Display(Name = "Prohry (sety)")]
    public int LostSets => SetsPlayed > 0 ? SetsPlayed - WonSets : 0;

    [Display(Name = "Body (prohry)")]
    public int PointsInLostSets { get; set; } = 0;
    public int SetsDifference => WonSets - LostSets;
}