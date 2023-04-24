using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedtableTemplateChecklistAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateDocumentType",
                table: "TemplateChecklists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TemplateChecklistAttachments",
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
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    TemplateChecklistId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateChecklistAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateChecklistAttachments_TemplateChecklists_TemplateChecklistId",
                        column: x => x.TemplateChecklistId,
                        principalTable: "TemplateChecklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklistAttachments_TemplateChecklistId",
                table: "TemplateChecklistAttachments",
                column: "TemplateChecklistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateChecklistAttachments");

            migrationBuilder.DropColumn(
                name: "TemplateDocumentType",
                table: "TemplateChecklists");
        }
    }
}
