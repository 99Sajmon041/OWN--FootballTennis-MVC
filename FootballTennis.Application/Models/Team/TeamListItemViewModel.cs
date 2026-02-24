using FootballTennis.Application.Models.Player;
using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Team;

public sealed class TeamListItemViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název")]
    public string Name { get; set; } = default!;

    [Display(Name = "Hráči")]
    public List<PlayerViewModel> TeamPlayers { get; set; } = [];
}
