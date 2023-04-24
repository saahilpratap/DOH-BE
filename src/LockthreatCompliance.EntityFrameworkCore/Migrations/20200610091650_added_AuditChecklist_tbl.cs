using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditChecklist_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditChecklistId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditChecklists",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ChecklistTitle = table.Column<string>(nullable: true),
                    AppEntityType = table.Column<int>(nullable: false),
                    EditorData = table.Column<string>(maxLength: 9999, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditChecklists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditChecklistId",
                table: "DocumentPaths",
                column: "AuditChecklistId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditChecklists_AuditChecklistId",
                table: "DocumentPaths",
                column: "AuditChecklistId",
                principalTable: "AuditChecklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditChecklists_AuditChecklistId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditChecklists");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditChecklistId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditChecklistId",
                table: "DocumentPaths");
        }
    }
}
