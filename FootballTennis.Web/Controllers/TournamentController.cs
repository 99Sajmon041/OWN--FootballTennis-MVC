using FootballTennis.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    public class TournamentController(ITournamentService tournamentService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await tournamentService.GetAllTournamentsAsync(CancellationToken.None);
            return View(model);
        }
    }
}
