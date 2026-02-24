using FootballTennis.Application.Models.Player;

namespace FootballTennis.Web.Utilities;

public static class CollectionHelper
{
    public static string GetTeamPlayersAsString(List<PlayerViewModel> TeamPlayers)
    {
        List<string> names = [];

        foreach (var player in TeamPlayers)
            names.Add(player.FullName);

        return string.Join(", ", names);
    }
}
