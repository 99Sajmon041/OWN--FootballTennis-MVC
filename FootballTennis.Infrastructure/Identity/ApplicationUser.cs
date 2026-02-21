using Microsoft.AspNetCore.Identity;


namespace FootballTennis.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public DateTime LastActivityUtc { get; set; }
    public bool IsLoggedIn { get; set; }
}
