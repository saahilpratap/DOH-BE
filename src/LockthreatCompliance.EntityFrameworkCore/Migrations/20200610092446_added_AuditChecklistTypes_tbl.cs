using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditChecklistTypes_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditChecklistTypes",
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
                    ChecklistTypeId = table.Column<int>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklistTypes_ChecklistTypeId",
                table: "AuditChecklistTypes",
                column: "ChecklistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklistTypes_ExternalAssessmentQuestionId",
                table: "AuditChecklistTypes",
                column: "ExternalAssessmentQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditChecklistTypes");
        }
    }
}
