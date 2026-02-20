using FootballTennis.Domain.Enums;

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
}
