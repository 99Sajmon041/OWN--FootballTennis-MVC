using FootballTennis.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    public class TournamentController(ITournamentService tournamentService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var model = await tournamentService.GetAllTournamentsAsync(ct);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
