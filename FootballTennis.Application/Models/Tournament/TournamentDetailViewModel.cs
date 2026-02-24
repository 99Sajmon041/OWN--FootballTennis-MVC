using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Models.Team;
using FootballTennis.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentDetailViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název turnaje")]
    public string Name { get; set; } = default!;

    [Display(Name = "Adresa konání")]
    public string Address { get; set; } = default!;

    [Display(Name = "Datum konání")]
    public DateTime Date { get; set; }

    [Display(Name = "Stav")]
    public Status Status { get; set; }

    [Display(Name = "Počet hráčů v teamu")]
    public int TeamPlayersCount { get; set; }

    [Display(Name = "Počet teamů")]
    public int TeamsCount => Teams.Count;

    [Display(Name = "Počet kol")]
    public int RoundsCount => TeamsCount > 1 ? Teams.Count - 1 : 0;

    [Display(Name = "Počet zápasů")]
    public int MatchesCount => Matches.Count;
    public List<TeamListItemViewModel> Teams { get; set; } = [];
    public List<MatchListItemViewModel> Matches { get; set; } = [];
}
