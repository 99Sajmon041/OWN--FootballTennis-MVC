using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentStatisticsViewModel
{
    public int TournamentId { get; set; }

    [Display(Name = "Název turnaje")]
    public string Name { get; set; } = default!;
    public List<TournamentStatisticsListItemViewModel> TeamsStatistics { get; set; } = [];
}
