namespace FootballTennis.Domain.Entities;

public sealed class TeamPlayer
{
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; } = default!;
    public int TeamId { get; set; }
    public Team Team { get; set; } = default!;
    public int PlayerId { get; set; }
    public Player Player { get; set; } = default!;
}
