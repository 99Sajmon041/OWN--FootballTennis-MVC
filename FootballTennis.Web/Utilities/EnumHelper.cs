using FootballTennis.Shared.Enums;

namespace FootballTennis.Web.Utilities;

public static class EnumHelper
{
    public static string GetCzechStatusName(Status status)
    {
        return status switch
        {
            Status.Scheduled => "Naplánovaný",
            Status.InProgress => "Aktivní",
            Status.Finished => "Skončený",
            _ => "Neznámý"
        };
    }

    public static string GetCzechMatchStatusName(MatchStatus status)
    {
        return status switch
        {
            MatchStatus.NotPlayed => "Neodehráno",
            MatchStatus.Playing => "Rozehráno",
            MatchStatus.Played => "Dohráno",
            _ => "Neznámo"
        };
    }
}
