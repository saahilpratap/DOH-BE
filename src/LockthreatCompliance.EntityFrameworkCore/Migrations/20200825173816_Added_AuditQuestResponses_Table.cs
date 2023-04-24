using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_AuditQuestResponses_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditQuestResponses",
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
                    AuditProjectId = table.Column<long>(nullable: true),
                    QuestionGroupId = table.Column<long>(nullable: true),
                    FlagValue = table.Column<int>(nullable: true),
                    ScoreValue = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditQuestResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditQuestResponses_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditQuestResponses_QuestionGroups_QuestionGroupId",
                        column: x => x.QuestionGroupId,
                        principalTable: "QuestionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditQuestResponses_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditQuestResponses_AuditProjectId",
                table: "AuditQuestResponses",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditQuestResponses_QuestionGroupId",
                table: "AuditQuestResponses",
                column: "QuestionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditQuestResponses_QuestionId",
                table: "AuditQuestResponses",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditQuestResponses");
        }
    }
}
