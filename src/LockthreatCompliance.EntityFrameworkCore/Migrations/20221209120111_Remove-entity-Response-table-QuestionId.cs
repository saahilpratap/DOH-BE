using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class RemoveentityResponsetableQuestionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
