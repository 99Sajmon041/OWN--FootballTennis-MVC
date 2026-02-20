using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballTennis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTeamPlayerCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamPlayersCount",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamPlayersCount",
                table: "Tournaments");
        }
    }
}
