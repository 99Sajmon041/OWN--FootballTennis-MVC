using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Match;

public sealed class MatchListItemViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Tým 1")]
    public string TeamOneName { get; set; } = default!;

    [Display(Name = "tým 2")]
    public string TeamTwoName { get; set; } = default!;

    [Display(Name = "Odehrán")]
    public bool IsPlayed { get; set; }

    [Display(Name = "Skóre")]
    public string? ScoreText { get; set; }

    [Display(Name = "Pořadí zápasu")]
    public int Order { get; set; }
}
