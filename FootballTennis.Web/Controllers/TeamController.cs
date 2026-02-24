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
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(int tournamentId, int teamPlayersCount, CancellationToken ct)
        {
            return View(new CreateTeamViewModel
            {
                TournamentId = tournamentId,
                TeamPlayersCount = teamPlayersCount,
                Players = await teamService.GetPlayersForTeamCreateAsync(ct)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(CreateTeamViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                model.Players = await teamService.GetPlayersForTeamCreateAsync(ct);
                return View(model);
            }



            return View();
        }
    }
}
