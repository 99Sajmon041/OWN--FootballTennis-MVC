using FootballTennis.Application.Models.Set;
using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Match;

public sealed class MatchUpdateViewModel
{
    public int Id { get; set; }

    public int TournamentId { get; set; }
    public int TeamOneId { get; set; }

    [Display(Name = "Název týmu 1.")]
    public string TeamOneName { get; set; } = default!;
    public int TeamTwoId { get; set; }

    [Display(Name = "Název týmu 2.")]
    public string TeamTwoName { get; set; } = default!;

    [Display(Name = "Pořadí zápasu")]
    public int Order { get; set; }
    public List<SetUpdateListItemViewModel> Sets { get; set; } = [];
}
