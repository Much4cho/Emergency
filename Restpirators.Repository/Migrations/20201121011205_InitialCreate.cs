using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Restpirators.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmergencyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emergencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmergencyTypeId = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReportTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ModUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emergencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emergencies_EmergencyTypes_EmergencyTypeId",
                        column: x => x.EmergencyTypeId,
                        principalTable: "EmergencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emergencies_EmergencyTypeId",
                table: "Emergencies",
                column: "EmergencyTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emergencies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EmergencyTypes");
        }
    }
}
