using FootballTennis.Application.Common.Exceptions;
using FootballTennis.Application.Models.Account;
using FootballTennis.Application.Services.Interfaces;
using FootballTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FootballTennis.Application.Services.Implementations;

public sealed class AccountService(
    ILogger<AccountService> logger,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IAccountService
{
    public async Task<bool> LoginAsync(LoginViewModel model, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(model.Email); 
        if (user is null)
        { 
            logger.LogInformation("User tries to Log-in but e-mail does not exists. Email: {Email}", model.Email);
            return false;
        }

        if (user.IsLoggedIn)
        {
            logger.LogWarning("Login blocked. User {Email} is already logged in. Last activity: {LastActivityUtc}",user.Email, user.LastActivityUtc); 
            throw new ConflictException("Uživatel je již přihlášen jinde - nejprve je třeba se odhlásit na jiném zařízení. ");
        }

        var loginResult = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (!loginResult.Succeeded) 
        {
            logger.LogInformation("User tries to Log-in but credentials are worng. Logging e-mail: {Email}", model.Email);
            return false;
        }

        user.IsLoggedIn = true;
        user.LastActivityUtc = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        var claims = await userManager.GetClaimsAsync(user);
        if (!claims.Any(x => x.Type == "FullName"))
        {
            await userManager.AddClaimAsync(user, new Claim("FullName", "Admin"));
        }

        await signInManager.RefreshSignInAsync(user);

        return true;
    }

    public async Task LogoutAsync(string userId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var appUser = await userManager.FindByIdAsync(userId);
        if (appUser is null)
        {
            return;
        }

        appUser.IsLoggedIn = false;
        await userManager.UpdateAsync(appUser);

        await signInManager.SignOutAsync();
    }
}
