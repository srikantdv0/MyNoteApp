using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class Removenavigationfromuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDST",
                value: new DateTime(2023, 5, 15, 8, 42, 57, 132, DateTimeKind.Utc).AddTicks(7330));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDST",
                value: new DateTime(2023, 5, 15, 8, 42, 57, 132, DateTimeKind.Utc).AddTicks(7330));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDST",
                value: new DateTime(2023, 4, 29, 1, 42, 3, 629, DateTimeKind.Utc).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDST",
                value: new DateTime(2023, 4, 29, 1, 42, 3, 629, DateTimeKind.Utc).AddTicks(7550));
        }
    }
}
