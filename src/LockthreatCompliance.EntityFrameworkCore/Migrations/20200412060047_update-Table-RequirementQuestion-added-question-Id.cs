using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updateTableRequirementQuestionaddedquestionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "RequirementQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequirementQuestion_QuestionId",
                table: "RequirementQuestion",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequirementQuestion_Questions_QuestionId",
                table: "RequirementQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequirementQuestion_Questions_QuestionId",
                table: "RequirementQuestion");

            migrationBuilder.DropIndex(
                name: "IX_RequirementQuestion_QuestionId",
                table: "RequirementQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "RequirementQuestion");
        }
    }
}
