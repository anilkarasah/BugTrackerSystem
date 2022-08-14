using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerAPI.Migrations
{
    public partial class BugModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "Bugs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Bugs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Bugs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_UserID",
                table: "Bugs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Users_UserID",
                table: "Bugs",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Users_UserID",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_UserID",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Bugs");
        }
    }
}
