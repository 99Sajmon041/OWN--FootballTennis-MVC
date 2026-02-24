using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Player;

public sealed class PlayerViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Jméno")]
    public string FullName { get; set; } = default!;
}
