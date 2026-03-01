using FootballTennis.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentListItemViewModel
{
    [Display(Name= "ID")]
    public int Id { get; set; }

    [Display(Name = "Název turnaje")]
    public string Name { get; set; } = default!;

    [Display(Name = "Místo konání")]
    public string Address { get; set; } = default!;

    [Display(Name = "Datum")]
    public DateTime Date { get; set; }

    [Display(Name = "Status")]
    public Status Status { get; set; }

    [Display(Name = "Počet hráčů v teamu")]
    public int TeamPlayersCount { get; set; }

    [Display(Name = "Teamů")]
    public int TeamsCount { get; set; }

    [Display(Name = "Počet zápasů")]
    public int MatchesCount { get; set; }

    [Display(Name = "Vítěz")]
    public string? WinnerName { get; set; }
}
