using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableAssessmentsAndAssessmentAgreementResponses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    BusinessEntityName = table.Column<string>(nullable: true),
                    ReportingDeadLine = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SendEmailNotification = table.Column<bool>(nullable: false),
                    SendSmsNotification = table.Column<bool>(nullable: false),
                    Info = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    AuthoritativeDocumentName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Feedback = table.Column<string>(nullable: true),
                    ReviewScore = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    HasFetchedLastAnswers = table.Column<bool>(nullable: false),
                    GeneralComplianceAssessmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessments_GeneralComplianceAssessments_GeneralComplianceAssessmentId",
                        column: x => x.GeneralComplianceAssessmentId,
                        principalTable: "GeneralComplianceAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentAgreementResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    AssessmentId = table.Column<int>(nullable: false),
                    HasAccepted = table.Column<bool>(nullable: false),
                    Signature = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAgreementResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentAgreementResponses_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentAgreementResponses_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentAgreementResponses_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAgreementResponses_AssessmentId",
                table: "AssessmentAgreementResponses",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAgreementResponses_BusinessEntityId",
                table: "AssessmentAgreementResponses",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAgreementResponses_UserId",
                table: "AssessmentAgreementResponses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_BusinessEntityId",
                table: "Assessments",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_GeneralComplianceAssessmentId",
                table: "Assessments",
                column: "GeneralComplianceAssessmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentAgreementResponses");

            migrationBuilder.DropTable(
                name: "Assessments");
        }
    }
}
