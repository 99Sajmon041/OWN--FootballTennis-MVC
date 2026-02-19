using FootballTennis.Domain.Entities;
using FootballTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballTennis.Infrastructure.Database;

public sealed class FootballTennisDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tournament> Tournaments { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
    public DbSet<Match> Matches { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;
    public DbSet<TeamPlayer> TeamPlayers { get; set; } = default!;
    public DbSet<Set> Sets { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tournament>(entity =>
        {
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(x => x.Date);
            entity.HasIndex(x => x.Status);

            entity.HasMany(x => x.Matches)
                .WithOne(x => x.Tournament)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Teams)
                .WithOne(x => x.Tournament)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Team>(entity =>
        {
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => new { x.TournamentId, x.Name }).IsUnique();
            entity.HasIndex(x => x.Position);
        });

        builder.Entity<Match>(entity =>
        {
            entity.HasOne(x => x.TeamOne)
                .WithMany()
                .HasForeignKey(x => x.TeamOneId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TeamTwo)
                .WithMany()
                .HasForeignKey(x => x.TeamTwoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(x => x.Sets)
                .WithOne(x => x.Match)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Tournament)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => new {x.TournamentId, x.Order });
            entity.HasIndex(x => new { x.TournamentId, x.TeamOneId, x.TeamTwoId }).IsUnique();
        });

        builder.Entity<Player>(entity =>
        {
            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.FullName)
                .IsUnique();

            entity.HasMany(x => x.TeamPlayers)
                .WithOne(x => x.Player)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<TeamPlayer>(entity =>
        {
            entity.HasKey(x => new { x.TournamentId, x.TeamId, x.PlayerId });

            entity.HasOne(x => x.Team)
                .WithMany(x => x.TeamPlayers)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Player)
                .WithMany(x => x.TeamPlayers)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Tournament)
                .WithMany()
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Set>(entity =>
        {
            entity.HasIndex(x => new { x.MatchId, x.SetNumber }).IsUnique();
        });
    }
}
