using FootballTennis.Shared.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FootballTennis.Application.Models.Tournament;

public sealed class TournamentsIndexViewModel
{
    public PagedResult<TournamentListItemViewModel> Result { get; set; } = default!;
    public List<SelectListItem> SortOptions { get; set; } = [];
}
