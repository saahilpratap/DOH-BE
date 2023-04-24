using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class removed_unnecessary_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDocSubModelsPath_AuditChecklists_AuditChecklistId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditDocSubModelsPath_AuditTemplates_AuditTemplateId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditChecklists_AuditChecklistId",
                table: "DocumentPaths");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditTemplates_AuditTemplateId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditChecklists");

            migrationBuilder.DropTable(
                name: "AuditChecklistTypes");

            migrationBuilder.DropTable(
                name: "AuditTemplates");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditChecklistId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditTemplateId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_AuditDocSubModelsPath_AuditChecklistId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropIndex(
                name: "IX_AuditDocSubModelsPath_AuditTemplateId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropColumn(
                name: "AuditChecklistId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditTemplateId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditChecklistId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropColumn(
                name: "AuditTemplateId",
                table: "AuditDocSubModelsPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditChecklistId",
                table: "DocumentPaths",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "DocumentPaths",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditTemplateId",
                table: "DocumentPaths",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditChecklistId",
                table: "AuditDocSubModelsPath",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditTemplateId",
                table: "AuditDocSubModelsPath",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditChecklists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppEntityType = table.Column<int>(type: "int", nullable: false),
                    AuditProjectId = table.Column<long>(type: "bigint", nullable: true),
                    ChecklistTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditorData = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditChecklists_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditChecklistTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalAssessmentQuestionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditChecklistTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditChecklistTypes_AbpDynamicParameterValues_ChecklistTypeId",
                        column: x => x.ChecklistTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditChecklistTypes_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppEntityType = table.Column<int>(type: "int", nullable: false),
                    AuditProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditorData = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TemplateTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditTemplates_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditChecklistId",
                table: "DocumentPaths",
                column: "AuditChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditTemplateId",
                table: "DocumentPaths",
                column: "AuditTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditChecklistId",
                table: "AuditDocSubModelsPath",
                column: "AuditChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditTemplateId",
                table: "AuditDocSubModelsPath",
                column: "AuditTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklists_AuditProjectId",
                table: "AuditChecklists",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklistTypes_ChecklistTypeId",
                table: "AuditChecklistTypes",
                column: "ChecklistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklistTypes_ExternalAssessmentQuestionId",
                table: "AuditChecklistTypes",
                column: "ExternalAssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTemplates_AuditProjectId",
                table: "AuditTemplates",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDocSubModelsPath_AuditChecklists_AuditChecklistId",
                table: "AuditDocSubModelsPath",
                column: "AuditChecklistId",
                principalTable: "AuditChecklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDocSubModelsPath_AuditTemplates_AuditTemplateId",
                table: "AuditDocSubModelsPath",
                column: "AuditTemplateId",
                principalTable: "AuditTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditChecklists_AuditChecklistId",
                table: "DocumentPaths",
                column: "AuditChecklistId",
                principalTable: "AuditChecklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditTemplates_AuditTemplateId",
                table: "DocumentPaths",
                column: "AuditTemplateId",
                principalTable: "AuditTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
