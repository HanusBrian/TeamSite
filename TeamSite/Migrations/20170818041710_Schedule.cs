using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeamSite.Migrations
{
    public partial class Schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Team_TeamId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Program_Client_ClientID",
                table: "Program");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_AspNetUsers_AccountManagerId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Client_ClientID",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_AspNetUsers_OpsSpecialistId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Program_ProgramId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Team_TeamId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_AspNetUsers_TeamManagerId",
                table: "Schedule");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_AccountManagerId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ClientID",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_OpsSpecialistId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ProgramId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_TeamId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_TeamManagerId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Program_ClientID",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Client_TeamId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AccountManagerId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "EcoSureClientID",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Schedule",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "TeamManagerId",
                table: "Schedule",
                newName: "CreateUser");

            migrationBuilder.RenameColumn(
                name: "OpsSpecialistId",
                table: "Schedule",
                newName: "ChangeUser");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "Program",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "Schedule",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Schedule",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateUser",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangeUser",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountManager",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeDate",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Complete",
                table: "Schedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Schedule",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExcelIndex",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpsSpecialist",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeDate",
                table: "Program",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangeUser",
                table: "Program",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Client",
                table: "Program",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Program",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "Program",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Client",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeDate",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangeUser",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RevenueRank",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangeUser",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Program_Client",
                table: "Program",
                column: "Client");

            migrationBuilder.AddForeignKey(
                name: "FK_Program_Client_Client",
                table: "Program",
                column: "Client",
                principalTable: "Client",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Program_Client_Client",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Program_Client",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "AccountManager",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ChangeDate",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Complete",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ExcelIndex",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "OpsSpecialist",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ChangeDate",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "ChangeUser",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "ChangeDate",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ChangeUser",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "RevenueRank",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ChangeDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChangeUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Schedule",
                newName: "ClientID");

            migrationBuilder.RenameColumn(
                name: "CreateUser",
                table: "Schedule",
                newName: "TeamManagerId");

            migrationBuilder.RenameColumn(
                name: "ChangeUser",
                table: "Schedule",
                newName: "OpsSpecialistId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Program",
                newName: "ReferenceNumber");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "TeamManagerId",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OpsSpecialistId",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountManagerId",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientID",
                table: "Program",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EcoSureClientID",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountManagersId = table.Column<string>(nullable: true),
                    ManagerId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OpsSpecialistsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_AccountManagersId",
                        column: x => x.AccountManagersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_OpsSpecialistsId",
                        column: x => x.OpsSpecialistsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_AccountManagerId",
                table: "Schedule",
                column: "AccountManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ClientID",
                table: "Schedule",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_OpsSpecialistId",
                table: "Schedule",
                column: "OpsSpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ProgramId",
                table: "Schedule",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeamId",
                table: "Schedule",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeamManagerId",
                table: "Schedule",
                column: "TeamManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ClientID",
                table: "Program",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TeamId",
                table: "Client",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_AccountManagersId",
                table: "Team",
                column: "AccountManagersId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ManagerId",
                table: "Team",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_OpsSpecialistsId",
                table: "Team",
                column: "OpsSpecialistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Team_TeamId",
                table: "Client",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Program_Client_ClientID",
                table: "Program",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_AspNetUsers_AccountManagerId",
                table: "Schedule",
                column: "AccountManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Client_ClientID",
                table: "Schedule",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_AspNetUsers_OpsSpecialistId",
                table: "Schedule",
                column: "OpsSpecialistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Program_ProgramId",
                table: "Schedule",
                column: "ProgramId",
                principalTable: "Program",
                principalColumn: "ProgramId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Team_TeamId",
                table: "Schedule",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_AspNetUsers_TeamManagerId",
                table: "Schedule",
                column: "TeamManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
