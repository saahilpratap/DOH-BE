using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableReviewDatasAndReviewQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(nullable: true),
                    ExternalAssessmentId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    ResponseType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    RequestComment = table.Column<string>(nullable: true),
                    IsChangedSinceLastResponse = table.Column<bool>(nullable: false),
                    LastResponseType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewDatas_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewDatas_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewDatas_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    SelectedAnswerOptionId = table.Column<int>(nullable: true),
                    ReviewDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewQuestions_ReviewDatas_ReviewDataId",
                        column: x => x.ReviewDataId,
                        principalTable: "ReviewDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDatas_AssessmentId",
                table: "ReviewDatas",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDatas_ControlRequirementId",
                table: "ReviewDatas",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDatas_ExternalAssessmentId",
                table: "ReviewDatas",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewQuestions_QuestionId",
                table: "ReviewQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewQuestions_ReviewDataId",
                table: "ReviewQuestions",
                column: "ReviewDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewQuestions");

            migrationBuilder.DropTable(
                name: "ReviewDatas");
        }
    }
}
