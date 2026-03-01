using FootballTennis.Shared.Enums;

namespace FootballTennis.Shared.ReadModels;

public sealed class TournamentListItemReadModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public int TeamPlayersCount { get; set; }
    public int TeamsCount { get; set; }
    public int MatchesCount { get; set; }
    public string? WinnerName { get; set; }
}
