using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballTennis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWinnerColumntoTournamentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnerId1",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_WinnerId1",
                table: "Tournaments",
                column: "WinnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Teams_WinnerId1",
                table: "Tournaments",
                column: "WinnerId1",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Teams_WinnerId1",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_WinnerId1",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "WinnerId1",
                table: "Tournaments");
        }
    }
}
