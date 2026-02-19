namespace FootballTennis.Domain.Entities;

public sealed class Set
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; } = default!;
    public int SetNumber { get; set; }
    public int? ScoreTeam1 { get; set; }
    public int? ScoreTeam2 { get; set; }
}
