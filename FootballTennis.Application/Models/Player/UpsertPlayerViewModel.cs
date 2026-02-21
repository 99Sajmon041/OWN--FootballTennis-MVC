using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Player;

public sealed class UpsertPlayerViewModel
{
    public int Id { get; set; }

    [Display(Name = "Jméno")]
    [Required(ErrorMessage = "Jméno hráče je povinné.")]
    [MaxLength(50, ErrorMessage = "Jmeno musí mít maximálně 50 znaků.")]
    public string FullName { get; set; } = default!;
}
