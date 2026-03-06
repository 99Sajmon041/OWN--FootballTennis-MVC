namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentStatisticsListItemViewModel
{
    public int TeamId { get; set; }
    public int Position { get; set; }
    public string TeamName { get; set; } = default!;
    public int MatchesPlayed { get; set; }
    public int WonMatches { get; set; }
    public int LostMatches => MatchesPlayed > 0 ? MatchesPlayed - WonMatches : 0;
    public int SetsDifference { get; set; }
    public int? PointsInLostSets { get; set; } = 0;
}