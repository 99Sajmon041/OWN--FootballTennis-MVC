using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Set;

public sealed class SetUpdateListItemViewModel
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int SetNumber { get; set; }

    [Range(0, 25, ErrorMessage = "Skóre musí být v rozmezí 0 - 25.")]
    public int? ScoreTeam1 { get; set; }

    [Range(0, 25, ErrorMessage = "Skóre musí být v rozmezí 0 - 25.")]
    public int? ScoreTeam2 { get; set; }
}
