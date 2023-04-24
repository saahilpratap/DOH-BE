using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedEntityresponsetableQuestionIdnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntityResponses_TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses",
                column: "TableTopExerciseQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableTopExerciseEntityResponses_TableTopExerciseQuestions_TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses",
                column: "TableTopExerciseQuestionId",
                principalTable: "TableTopExerciseQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableTopExerciseEntityResponses_TableTopExerciseQuestions_TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses");

            migrationBuilder.DropIndex(
                name: "IX_TableTopExerciseEntityResponses_TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses");

            migrationBuilder.DropColumn(
                name: "TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses");
        }
    }
}
