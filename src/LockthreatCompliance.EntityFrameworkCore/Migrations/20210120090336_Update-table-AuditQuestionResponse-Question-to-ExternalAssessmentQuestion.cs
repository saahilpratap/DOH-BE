using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableAuditQuestionResponseQuestiontoExternalAssessmentQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditQuestResponses_Questions_QuestionId",
                table: "AuditQuestResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditQuestResponses_ExternalAssementQuestions_QuestionId",
                table: "AuditQuestResponses",
                column: "QuestionId",
                principalTable: "ExternalAssementQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditQuestResponses_ExternalAssementQuestions_QuestionId",
                table: "AuditQuestResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditQuestResponses_Questions_QuestionId",
                table: "AuditQuestResponses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
