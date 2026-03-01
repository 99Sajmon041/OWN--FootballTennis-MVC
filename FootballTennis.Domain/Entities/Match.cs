using FootballTennis.Shared.Enums;
namespace FootballTennis.Domain.Entities;

public sealed class Match
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; } = default!;
    public int TeamOneId { get; set; }
    public Team TeamOne { get; set; } = default!;
    public int TeamTwoId { get; set; }
    public Team TeamTwo { get; set; } = default!;
    public Status Status { get; set; }
    public int Order { get; set; }
    public DateTime? PlayedAt { get; set; }
    public List<Set> Sets { get; set; } = [];
}
