using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class Ownertocreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_OwnerId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Notes",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                newName: "IX_Notes_CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 7, 26, 42, 184, DateTimeKind.Utc).AddTicks(4620));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 7, 26, 42, 184, DateTimeKind.Utc).AddTicks(4620));

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_CreatorId",
                table: "Notes",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_CreatorId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Notes",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_CreatorId",
                table: "Notes",
                newName: "IX_Notes_OwnerId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 7, 7, 6, 56, DateTimeKind.Utc).AddTicks(1120));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 7, 7, 6, 56, DateTimeKind.Utc).AddTicks(1130));

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_OwnerId",
                table: "Notes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
