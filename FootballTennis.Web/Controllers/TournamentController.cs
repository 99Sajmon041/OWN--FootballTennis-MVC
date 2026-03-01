using Azure.Core;
using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Tournament;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Shared.Enums;
using FootballTennis.Shared.Pagination;
using FootballTennis.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    public class TournamentController(ITournamentService tournamentService) : Controller
    {
        public async Task<IActionResult> Index(PagedRequest request, CancellationToken ct)
        {
            var model = new TournamentsIndexViewModel
            {
                Result = await tournamentService.GetAllTournamentsAsync(request, ct),
                SortOptions = OptionsBuilder.GetSortOptionsForTournaments()
            };
                        
            return View(model);
        }

        [Authorize(Roles = nameof(AdminRole.Admin))]
        public IActionResult Create()
        {
            return View(new TournamentUpsertViewModel { Date = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Create(TournamentUpsertViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await tournamentService.CreateTournamentAsync(model, ct);

                TempData["Success"] = "Turnaj úspěšně vytvořen.";

                return RedirectToAction(nameof(Index));
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(int id, CancellationToken ct)
        {
            var model = await tournamentService.GetTournamentForUpdateAsync(id, ct);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Update(TournamentUpsertViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await tournamentService.UpdateTournamentAsync(model.Id, model, ct);

                TempData["Success"] = "Turnaj úspěšně upraven.";

                return RedirectToAction(nameof(Index));
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(AdminRole.Admin))]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await tournamentService.DeleteTournamentAsync(id, ct);

            TempData["Success"] = "Turnaj úspěšně smazán.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, CancellationToken ct)
        {
            var model = await tournamentService.GetTournamentWithDetailsAsync(id, ct);
            return View(model);
        }
    }
}
