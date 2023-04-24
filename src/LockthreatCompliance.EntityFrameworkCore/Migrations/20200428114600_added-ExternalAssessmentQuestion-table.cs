using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedExternalAssessmentQuestiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssementQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AnswerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssementQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssementQuestions_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalQuestionAnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalQuestionAnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalQuestionAnswerOptions_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalQuestionAnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssementQuestions_ControlRequirementId",
                table: "ExternalAssementQuestions",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalQuestionAnswerOptions_ExternalAssessmentQuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "ExternalAssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalQuestionAnswerOptions_QuestionId",
                table: "ExternalQuestionAnswerOptions",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalQuestionAnswerOptions");

            migrationBuilder.DropTable(
                name: "ExternalAssementQuestions");
        }
    }
}
