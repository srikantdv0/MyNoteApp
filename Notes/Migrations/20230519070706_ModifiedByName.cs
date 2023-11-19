using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class ModifiedByName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDTS",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByName",
                table: "Notes",
                type: "TEXT",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "ModifiedByName",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDTS",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 6, 39, 47, 435, DateTimeKind.Utc).AddTicks(3660));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDTS",
                value: new DateTime(2023, 5, 19, 6, 39, 47, 435, DateTimeKind.Utc).AddTicks(3660));
        }
    }
}
