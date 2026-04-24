using FootballTennis.Application.Extensions;
using FootballTennis.Infrastructure.Database;
using FootballTennis.Infrastructure.Database.Extensions;
using FootballTennis.Infrastructure.Identity;
using FootballTennis.Infrastructure.SeedOptions;
using FootballTennis.Web.MiddleWare;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<FootballTennisDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("AuthPolicy", HttpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DefaultSeeder>();
    await seeder.SeedAdminAccount();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}