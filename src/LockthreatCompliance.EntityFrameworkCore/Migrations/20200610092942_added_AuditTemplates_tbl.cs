using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditTemplates_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditTemplateId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditTemplates",
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
                    TemplateTitle = table.Column<string>(nullable: true),
                    AppEntityType = table.Column<int>(nullable: false),
                    EditorData = table.Column<string>(maxLength: 9999, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditTemplateId",
                table: "DocumentPaths",
                column: "AuditTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditTemplates_AuditTemplateId",
                table: "DocumentPaths",
                column: "AuditTemplateId",
                principalTable: "AuditTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditTemplates_AuditTemplateId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditTemplates");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditTemplateId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditTemplateId",
                table: "DocumentPaths");
        }
    }
}
