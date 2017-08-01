using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeamSite.Migrations
{
    public partial class Teams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountTeamID",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountTeamID",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountTeams",
                columns: table => new
                {
                    AccountTeamID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTeams", x => x.AccountTeamID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountTeamID",
                table: "Employees",
                column: "AccountTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AccountTeamID",
                table: "Clients",
                column: "AccountTeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AccountTeams_AccountTeamID",
                table: "Clients",
                column: "AccountTeamID",
                principalTable: "AccountTeams",
                principalColumn: "AccountTeamID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AccountTeams_AccountTeamID",
                table: "Employees",
                column: "AccountTeamID",
                principalTable: "AccountTeams",
                principalColumn: "AccountTeamID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AccountTeams_AccountTeamID",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AccountTeams_AccountTeamID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "AccountTeams");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AccountTeamID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Clients_AccountTeamID",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AccountTeamID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AccountTeamID",
                table: "Clients");
        }
    }
}
