using System.ComponentModel.DataAnnotations;


namespace FootballTennis.Application.Models.Team;

public sealed class CreateTeamViewModel
{
    public int TournamentId { get; set; }

    [Required(ErrorMessage = "Název týmu je povinný.")]
    [StringLength(100, ErrorMessage = "Název týmu je povinný.", MinimumLength = 5)]
    [Display(Name = "Název týmu")]
    public string Name { get; set; } = default!;

    public int TeamPlayersCount { get; set; }
    public List<int> SelectedPlayersId { get; set; } = [];
    public List<PlayerOptionViewModel> Players { get; set; } = [];
}
