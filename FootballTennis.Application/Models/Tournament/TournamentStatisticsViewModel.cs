namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentStatisticsViewModel
{
    public int TournamentId { get; set; }
    public List<TournamentStatisticsListItemViewModel> TeamsStatistics { get; set; } = [];
}
