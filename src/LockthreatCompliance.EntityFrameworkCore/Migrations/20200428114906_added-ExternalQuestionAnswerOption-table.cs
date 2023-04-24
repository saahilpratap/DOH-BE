using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedExternalQuestionAnswerOptiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_Questions_QuestionId",
                table: "ExternalQuestionAnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_ExternalQuestionAnswerOptions_ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_ExternalAssementQuestions_QuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "QuestionId",
                principalTable: "ExternalAssementQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_ExternalAssementQuestions_QuestionId",
                table: "ExternalQuestionAnswerOptions");

            migrationBuilder.AddColumn<int>(
                name: "ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalQuestionAnswerOptions_ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "ExternalAssessmentQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "ExternalAssessmentQuestionId",
                principalTable: "ExternalAssementQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalQuestionAnswerOptions_Questions_QuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
