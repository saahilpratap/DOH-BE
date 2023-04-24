using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_ExternalAssessmentCRQuestionare_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessmentCRQuestionares",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ExternalAssessmentId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: false),
                    QuestionAddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentCRQuestionares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentCRQuestionares_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentCRQuestionares_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentCRQuestionares_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentCRQuestionares_ControlRequirementId",
                table: "ExternalAssessmentCRQuestionares",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentCRQuestionares_ExternalAssessmentId",
                table: "ExternalAssessmentCRQuestionares",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentCRQuestionares_ExternalAssessmentQuestionId",
                table: "ExternalAssessmentCRQuestionares",
                column: "ExternalAssessmentQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAssessmentCRQuestionares");
        }
    }
}
