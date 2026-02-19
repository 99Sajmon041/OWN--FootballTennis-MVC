using FootballTennis.Domain.Enums;

namespace FootballTennis.Domain.Entities;

public sealed class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public List<Team> Teams { get; set; } = [];
    public List<Match> Matches { get; set; } = [];
}
