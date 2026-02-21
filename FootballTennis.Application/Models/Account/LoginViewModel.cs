using System.ComponentModel.DataAnnotations;

namespace FootballTennis.Application.Models.Account;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "zajdete e-mail ve správném formátu.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "heslo je povinné.")]
    [MinLength(6, ErrorMessage = "Minimální délka hesla je 6 znaků.")]
    public string Password { get; set; } = default!;
}
