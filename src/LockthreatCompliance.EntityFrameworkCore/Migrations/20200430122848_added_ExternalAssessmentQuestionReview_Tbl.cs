using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_ExternalAssessmentQuestionReview_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalAssessmentQuestionReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ExternalAssessmentId = table.Column<int>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: false),
                    ReviewDataId = table.Column<int>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    SelectedAnswerOptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentQuestionReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentQuestionReviews_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentQuestionReviews_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentQuestionReviews_ReviewDatas_ReviewDataId",
                        column: x => x.ReviewDataId,
                        principalTable: "ReviewDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths",
                column: "ExternalAssessmentQuestionReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentQuestionReviews_ExternalAssessmentId",
                table: "ExternalAssessmentQuestionReviews",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentQuestionReviews_ExternalAssessmentQuestionId",
                table: "ExternalAssessmentQuestionReviews",
                column: "ExternalAssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentQuestionReviews_ReviewDataId",
                table: "ExternalAssessmentQuestionReviews",
                column: "ReviewDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_ExternalAssessmentQuestionReviews_ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths",
                column: "ExternalAssessmentQuestionReviewId",
                principalTable: "ExternalAssessmentQuestionReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_ExternalAssessmentQuestionReviews_ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "ExternalAssessmentQuestionReviews");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentQuestionReviewId",
                table: "DocumentPaths");
        }
    }
}
