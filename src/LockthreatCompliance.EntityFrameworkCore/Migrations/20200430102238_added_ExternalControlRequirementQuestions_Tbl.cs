using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_ExternalControlRequirementQuestions_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalControlRequirementQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: false),
                    QuestionAddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalControlRequirementQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalControlRequirementQuestions_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalControlRequirementQuestions_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalControlRequirementQuestions_ControlRequirementId",
                table: "ExternalControlRequirementQuestions",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalControlRequirementQuestions_ExternalAssessmentQuestionId",
                table: "ExternalControlRequirementQuestions",
                column: "ExternalAssessmentQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalControlRequirementQuestions");
        }
    }
}
