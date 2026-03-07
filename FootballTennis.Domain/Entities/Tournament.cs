using FootballTennis.Shared.Enums;

namespace FootballTennis.Domain.Entities;

public sealed class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public int TeamPlayersCount { get; set; }
    public Team? Winner { get; set; }
    public int? WinnerId { get; set; }
    public List<Team> Teams { get; set; } = [];
    public List<Match> Matches { get; set; } = [];
}
