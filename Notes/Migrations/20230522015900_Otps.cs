using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class Otps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Otps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(type: "INTEGER", maxLength: 6, nullable: false),
                    EmailId = table.Column<string>(type: "TEXT", nullable: false),
                    ValidTill = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 22, 1, 59, 0, 790, DateTimeKind.Utc).AddTicks(4210));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 22, 1, 59, 0, 790, DateTimeKind.Utc).AddTicks(4210));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Otps");

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
        }
    }
}
