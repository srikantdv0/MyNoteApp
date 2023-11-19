using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class DSTtoDTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDST",
                table: "Users",
                newName: "CreatedDTS");

            migrationBuilder.RenameColumn(
                name: "ModifiedDST",
                table: "Notes",
                newName: "ModifiedDTS");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDTS",
                table: "Users",
                newName: "CreatedDST");

            migrationBuilder.RenameColumn(
                name: "ModifiedDTS",
                table: "Notes",
                newName: "ModifiedDST");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDST",
                value: new DateTime(2023, 5, 16, 1, 20, 27, 541, DateTimeKind.Utc).AddTicks(1500));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDST",
                value: new DateTime(2023, 5, 16, 1, 20, 27, 541, DateTimeKind.Utc).AddTicks(1500));
        }
    }
}
