using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Team;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    [Route("Tournament/{tournamentId:int}/Teams")]
    public class TeamController(ITeamService teamService) : Controller
    {
        [HttpGet("Create")]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(int tournamentId, int teamPlayersCount, CancellationToken ct)
        {
            return View(new CreateTeamViewModel
            {
                TournamentId = tournamentId,
                TeamPlayersCount = teamPlayersCount,
                Players = await teamService.GetPlayersForTeamCreateAsync(tournamentId, ct)
            });
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(int tournamentId, CreateTeamViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                model.Players = await teamService.GetPlayersForTeamCreateAsync(model.TournamentId, ct);
                return View(model);
            }

            try
            {
                await teamService.AddTeamAsync(model, ct);
                return RedirectToAction("Detail", "Tournament", new { id = tournamentId });
            }
            catch (Exception ex) when (ex is ConflictException or DomainException)
            {
                TempData["Error"] = ex.Message;
                model.Players = await teamService.GetPlayersForTeamCreateAsync(tournamentId, ct);
            }

            return View(model);
        }

        [HttpPost("{id:int}/Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Delete(int tournamentId, int id, CancellationToken ct)
        {
            try
            {
                await teamService.DeleteTeamAsync(tournamentId, id, ct);
                TempData["Success"] = "Tým úspěšně smazán";

                return RedirectToAction("Detail", "Tournament", new { id = tournamentId });
            }
            catch (Exception ex) when (ex is ConflictException or DomainException)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Detail", "Tournament", new { id = tournamentId });
            }
        }
    }
}
