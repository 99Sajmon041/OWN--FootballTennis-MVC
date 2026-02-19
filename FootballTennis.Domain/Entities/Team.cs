namespace FootballTennis.Domain.Entities;

public sealed class Team
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int? Position { get; set; }
    public List<TeamPlayer> TeamPlayers { get; set; } = [];
}
