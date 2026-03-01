using FootballTennis.Shared.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballTennis.Application.Models.Player;

public sealed class PlayersIndexViewModel
{
    public PagedResult<PlayerStatsListItemViewModel> Result { get; set; } = default!;
    public List<SelectListItem> SortOptions { get; set; } = [];
}
 