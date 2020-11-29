using Microsoft.EntityFrameworkCore.Migrations;

namespace Restpirators.Repository.Migrations
{
    public partial class UpdateEmergencyAssignedToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToTeamId",
                table: "Emergencies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emergencies_AssignedToTeamId",
                table: "Emergencies",
                column: "AssignedToTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emergencies_Teams_AssignedToTeamId",
                table: "Emergencies",
                column: "AssignedToTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emergencies_Teams_AssignedToTeamId",
                table: "Emergencies");

            migrationBuilder.DropIndex(
                name: "IX_Emergencies_AssignedToTeamId",
                table: "Emergencies");

            migrationBuilder.DropColumn(
                name: "AssignedToTeamId",
                table: "Emergencies");
        }
    }
}
