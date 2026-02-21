using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Account;
using FootballTennis.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballTennis.Web.Controllers
{
    public class AccountController(IAccountService accountService) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginResult = await accountService.LoginAsync(model, ct);
                if (!loginResult)
                {
                    ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (ConflictException ex)
            {
                TempData["Info"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            await accountService.LogoutAsync(userId, ct);

            TempData["Success"] = "Uživatel úspěšně odhlášen.";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
