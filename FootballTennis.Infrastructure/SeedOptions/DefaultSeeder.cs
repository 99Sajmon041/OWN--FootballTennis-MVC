using FootballTennis.Domain.Enums;
using FootballTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FootballTennis.Infrastructure.SeedOptions;

public sealed class DefaultSeeder(
    ILogger<DefaultSeeder> logger,
    IOptions<AdminUser> options, 
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task SeedAdminAccount()
    {
        var admin = options.Value;
        ApplicationUser deafaultAccount;

        var roleExists = await roleManager.RoleExistsAsync(nameof(AdminRole.Admin));
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(AdminRole.Admin)));
           logger.LogInformation("Role admin was created successfully.");
        }

        var existAdmin = await userManager.FindByEmailAsync(admin.Email);
        if (existAdmin is null)
        {
            deafaultAccount = new ApplicationUser
            {
                FullName = admin.FullName,
                Email = admin.Email,
                UserName = admin.Email
            };

            var userResult = await userManager.CreateAsync(deafaultAccount, admin.Password);
            if (!userResult.Succeeded)
            {
                logger.LogError("Errors while create admin account: {Errors}.", string.Join(", ", userResult.Errors.Select(x => x.Description)));
                return;
            }
            else
            {
                logger.LogInformation("Admin account was created successfully.");
            }
        }
        else
        {
            deafaultAccount = existAdmin;
        }

        var hasUserAdminRole = await userManager.IsInRoleAsync(deafaultAccount, nameof(AdminRole.Admin));
        if (!hasUserAdminRole)
        {
            var roleResult = await userManager.AddToRoleAsync(deafaultAccount, nameof(AdminRole.Admin));
            if (!roleResult.Succeeded)
            {
                logger.LogError("Errors while adding role admin to admin account: {Errors}.", string.Join(", ", roleResult.Errors.Select(x => x.Description)));
            }
            else
            {
                logger.LogInformation("Admin role was added to admin account sucessfully.");
            }
        }
    }
}
