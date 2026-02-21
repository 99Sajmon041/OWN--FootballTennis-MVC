using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Player;

public sealed class PlayerStatsListItemViewModel
{
    [Display(Name = "ID")]
    public int PlayerId { get; set; }

    [Display(Name = "Celé jméno")]
    public string FullName { get; set; } = default!;

    [Display(Name = "Odehrané turnaje")]
    public int TournamentsCount { get; set; }

    [Display(Name = "Počet 1. míst")]
    public int FirstCount { get; set; }

    [Display(Name = "Počet 2. míst")]
    public int SecondCount { get; set; }

    [Display(Name = "Počet 3. míst")]
    public int ThirdCount { get; set; }
}
