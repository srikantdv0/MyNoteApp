using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class NameinUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Notes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDTS", "Name" },
                values: new object[] { new DateTime(2023, 5, 19, 6, 39, 47, 435, DateTimeKind.Utc).AddTicks(3660), "SrikantfromGmail" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDTS", "Name" },
                values: new object[] { new DateTime(2023, 5, 19, 6, 39, 47, 435, DateTimeKind.Utc).AddTicks(3660), "SrikantfromWf" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 3, "RWU" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 4, "RWUD" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 16, 16, 13, 34, 713, DateTimeKind.Utc).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 16, 16, 13, 34, 713, DateTimeKind.Utc).AddTicks(9030));
        }
    }
}
