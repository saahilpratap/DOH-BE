using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableQuestResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestResponses",
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
                    QuestionId = table.Column<int>(nullable: false),
                    ExternalAssessmentQuestionId = table.Column<int>(nullable: true),
                    ExternalAssessmentId = table.Column<int>(nullable: true),
                    SelfAssessmentQuestionId = table.Column<int>(nullable: true),
                    AuditProjectId = table.Column<long>(nullable: true),
                    QuestionGroupId = table.Column<long>(nullable: true),
                    ExternalAssessmentCRQuestionareId = table.Column<int>(nullable: true),
                    FlagValue = table.Column<int>(nullable: true),
                    ScoreValue = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestResponses_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestResponses_ExternalAssessmentCRQuestionares_ExternalAssessmentCRQuestionareId",
                        column: x => x.ExternalAssessmentCRQuestionareId,
                        principalTable: "ExternalAssessmentCRQuestionares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestResponses_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestResponses_ExternalAssementQuestions_ExternalAssessmentQuestionId",
                        column: x => x.ExternalAssessmentQuestionId,
                        principalTable: "ExternalAssementQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestResponses_QuestionGroups_QuestionGroupId",
                        column: x => x.QuestionGroupId,
                        principalTable: "QuestionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestResponses_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestResponses_Questions_SelfAssessmentQuestionId",
                        column: x => x.SelfAssessmentQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_AuditProjectId",
                table: "QuestResponses",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_ExternalAssessmentCRQuestionareId",
                table: "QuestResponses",
                column: "ExternalAssessmentCRQuestionareId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_ExternalAssessmentId",
                table: "QuestResponses",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_ExternalAssessmentQuestionId",
                table: "QuestResponses",
                column: "ExternalAssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_QuestionGroupId",
                table: "QuestResponses",
                column: "QuestionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_QuestionId",
                table: "QuestResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_SelfAssessmentQuestionId",
                table: "QuestResponses",
                column: "SelfAssessmentQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestResponses");
        }
    }
}
