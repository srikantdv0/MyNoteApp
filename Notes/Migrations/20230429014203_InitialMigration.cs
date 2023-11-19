using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    CreatedDST = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: false),
                    CreatedDTS = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LastModifieds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "SharedNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    SharedToId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedNotes_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedNotes_Users_SharedToId",
                        column: x => x.SharedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "R" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "RW" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 3, "RWU" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description" },
                values: new object[] { 4, "RWUD" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDST", "Email", "IsActive", "Password" },
                values: new object[] { 1, new DateTime(2023, 4, 29, 1, 42, 3, 629, DateTimeKind.Utc).AddTicks(7550), "srikantdv0@gmail.com", true, "Password123" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDST", "Email", "IsActive", "Password" },
                values: new object[] { 2, new DateTime(2023, 4, 29, 1, 42, 3, 629, DateTimeKind.Utc).AddTicks(7550), "srikant.yadav@gmail.com", true, "Srikant@123" });

            migrationBuilder.CreateIndex(
                name: "IX_LastModifieds_ModifiedBy",
                table: "LastModifieds",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LastModifieds_NoteId",
                table: "LastModifieds",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedNotes_NoteId",
                table: "SharedNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedNotes_PermissionId",
                table: "SharedNotes",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedNotes_SharedToId",
                table: "SharedNotes",
                column: "SharedToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastModifieds");

            migrationBuilder.DropTable(
                name: "SharedNotes");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
