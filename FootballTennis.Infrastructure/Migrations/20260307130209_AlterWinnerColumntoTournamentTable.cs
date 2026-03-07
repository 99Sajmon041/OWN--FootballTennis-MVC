using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballTennis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterWinnerColumntoTournamentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Teams_WinnerId1",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_WinnerId1",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "WinnerId1",
                table: "Tournaments");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_WinnerId",
                table: "Tournaments",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Teams_WinnerId",
                table: "Tournaments",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Teams_WinnerId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_WinnerId",
                table: "Tournaments");

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
    }
}
