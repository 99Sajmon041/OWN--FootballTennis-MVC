namespace FootballTennis.Application.Models.Team;

public sealed class PlayerOptionViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public bool IsAlreadyAssigned { get; set; }
}
