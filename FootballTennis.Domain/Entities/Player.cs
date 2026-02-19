namespace FootballTennis.Domain.Entities;

public sealed class Player
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public List<TeamPlayer> TeamPlayers { get; set; } = [];
}
