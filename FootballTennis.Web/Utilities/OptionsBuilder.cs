using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballTennis.Web.Utilities;

public static class OptionsBuilder
{
    public static List<SelectListItem> GetSortOptionsForPlayers()
    {
        return
        [
            new SelectListItem { Value = "FullName", Text = "Jméno" },
            new SelectListItem { Value = "TournamentsCount", Text = "Odehrané turnaje" },
            new SelectListItem { Value = "FirstCount", Text = "1. místo" },
            new SelectListItem { Value = "SecondCount", Text = "2. místo" },
            new SelectListItem { Value = "ThirdCount", Text = "3. místo" }
        ];
    }

    public static List<SelectListItem> GetPageSizeOptions()
    {
        return
        [
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" }
        ];
    }

    public static List<SelectListItem> GetSortOptionsForTournaments()
    {
        return
        [
            new SelectListItem { Value = "Name", Text = "Název" },
            new SelectListItem { Value = "Address", Text = "Adresa" },
            new SelectListItem { Value = "Date", Text = "Datum" },
            new SelectListItem { Value = "Status", Text = "Stav" },
            new SelectListItem { Value = "TeamPlayersCount", Text = "Počet hráčů v týmu" }
        ];
    }
}
