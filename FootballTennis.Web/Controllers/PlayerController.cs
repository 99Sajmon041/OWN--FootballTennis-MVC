using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Player;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    public class PlayerController(IPlayerService playerService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var model = await playerService.GetPlayerStatsListAsync(ct);
            return View(model);
        }

        [Authorize(Roles = nameof(AdminRole.Admin))]
        public IActionResult Create()
        {
            return View(new UpsertPlayerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(UpsertPlayerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await playerService.CreatePlayerAsync(model, ct);
            }
            catch (ConflictException ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }

            TempData["Success"] = "Hráč úspěšně vytvořen.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await playerService.DeletePlayerAsync(id, ct);

            TempData["Success"] = "Hráč úspěšně smazán.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(int id, CancellationToken ct)
        {
            var model = await playerService.GetPlayerForUpdateAsync(id, ct);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(UpsertPlayerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await playerService.UpdatePlayerAsync(model.Id, model, ct);

            TempData["Success"] = "Hráč úspěšně změněn.";

            return RedirectToAction(nameof(Index));
        }
    }
}