using FootballTennis.Domain.Enums;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentListItemViewModel
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
