using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableFeedbackDetailsandFeedbackDetailQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackDetailQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    FeedbackDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackDetailQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackDetailQuestions_FeedbackDetails_FeedbackDetailId",
                        column: x => x.FeedbackDetailId,
                        principalTable: "FeedbackDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackDetailQuestions_FeedBackQuestioner_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FeedBackQuestioner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetailQuestions_FeedbackDetailId",
                table: "FeedbackDetailQuestions",
                column: "FeedbackDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetailQuestions_QuestionId",
                table: "FeedbackDetailQuestions",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackDetailQuestions");

            migrationBuilder.DropTable(
                name: "FeedbackDetails");
        }
    }
}
