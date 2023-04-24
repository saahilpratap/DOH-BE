using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_AssessmentRequestClarifications_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentRequestClarifications",
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
                    AssessmentId = table.Column<int>(nullable: true),
                    ExternalAssessmentId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ControlRequirementId = table.Column<int>(nullable: true),
                    ReviewDataId = table.Column<int>(nullable: true),
                    AuthorityComment = table.Column<string>(maxLength: 9999, nullable: true),
                    ResponseComment = table.Column<string>(maxLength: 9999, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ClarificationNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentRequestClarifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentRequestClarifications_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentRequestClarifications_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentRequestClarifications_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentRequestClarifications_ReviewDatas_ReviewDataId",
                        column: x => x.ReviewDataId,
                        principalTable: "ReviewDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentRequestClarifications_AssessmentId",
                table: "AssessmentRequestClarifications",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentRequestClarifications_ControlRequirementId",
                table: "AssessmentRequestClarifications",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentRequestClarifications_ExternalAssessmentId",
                table: "AssessmentRequestClarifications",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentRequestClarifications_ReviewDataId",
                table: "AssessmentRequestClarifications",
                column: "ReviewDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentRequestClarifications");
        }
    }
}
