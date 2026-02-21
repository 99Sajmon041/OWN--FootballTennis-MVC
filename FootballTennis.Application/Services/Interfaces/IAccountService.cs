using FootballTennis.Application.Models.Account;
using FootballTennis.Infrastructure.Identity;

namespace FootballTennis.Application.Services.Interfaces;

public interface IAccountService
{
    Task<bool> LoginAsync(LoginViewModel model, CancellationToken ct);
    Task LogoutAsync(string userId, CancellationToken ct);
}
