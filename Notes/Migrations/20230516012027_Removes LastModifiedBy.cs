using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class RemovesLastModifiedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastModifieds");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDST",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "ModifiedDST",
                table: "Notes");

            migrationBuilder.CreateTable(
                name: "LastModifieds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifiedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedDST = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastModifieds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LastModifieds_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LastModifieds_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_LastModifieds_ModifiedBy",
                table: "LastModifieds",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LastModifieds_NoteId",
                table: "LastModifieds",
                column: "NoteId");
        }
    }
}
