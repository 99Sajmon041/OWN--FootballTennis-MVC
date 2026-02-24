using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentUpsertViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název turnaje")]
    [Required(ErrorMessage = "Název turnaje je povinný.")]
    [MaxLength(100, ErrorMessage = "Název musí mít max. 100 znaků.")]
    public string Name { get; set; } = default!;

    [Display(Name = "Místo konání")]
    [Required(ErrorMessage = "Adresa turnaje je povinná.")]
    [MaxLength(200, ErrorMessage = "Název musí mít max. 200 znaků.")]
    public string Address { get; set; } = default!;

    [Display(Name = "Datum konání")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Display(Name = "Počet hráčů")]
    [Range(2, 4, ErrorMessage = "Povolený počet hráčů v teamu je 2 - 4.")]
    public int TeamPlayersCount { get; set; }
}
