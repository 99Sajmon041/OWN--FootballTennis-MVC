using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Match;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    [Route("Tournament/{tournamentId:int}/Matches")]
    public class MatchController(IMatchService matchService) : Controller
    {
        [HttpGet("{id:int}/Update")]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(int id, int tournamentId, CancellationToken ct)
        {
            var model = await matchService.GetMatchForUpdateAsync(id, tournamentId, ct);
            return View(model);
        }

        [HttpPost("{id:int}/Update")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(int id, int tournamentId, MatchUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await matchService.UpdateMatchAsync(id, tournamentId, model, ct);
                TempData["Success"] = "Zápas úspěšně upraven";
                return RedirectToAction(nameof(Update), new { id, tournamentId });
            }
            catch(ConflictException ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }
    }
}
