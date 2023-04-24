using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableQuestionGroupaddednewcolumsQuestionnaireStageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionnaireStageId",
                table: "QuestionGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroups_QuestionnaireStageId",
                table: "QuestionGroups",
                column: "QuestionnaireStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionGroups_AbpDynamicParameterValues_QuestionnaireStageId",
                table: "QuestionGroups",
                column: "QuestionnaireStageId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionGroups_AbpDynamicParameterValues_QuestionnaireStageId",
                table: "QuestionGroups");

            migrationBuilder.DropIndex(
                name: "IX_QuestionGroups_QuestionnaireStageId",
                table: "QuestionGroups");

            migrationBuilder.DropColumn(
                name: "QuestionnaireStageId",
                table: "QuestionGroups");
        }
    }
}
