using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableFindingReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindingReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    FindingReportClassificationId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    FindingAction = table.Column<int>(nullable: false),
                    OtherCategoryName = table.Column<string>(nullable: true),
                    AssessmentId = table.Column<int>(nullable: true),
                    FinderId = table.Column<long>(nullable: true),
                    AuditorId = table.Column<long>(nullable: true),
                    AssignedToUserId = table.Column<long>(nullable: true),
                    FindingCoordinatorId = table.Column<long>(nullable: true),
                    FindingOwnerId = table.Column<long>(nullable: true),
                    FindingManagerId = table.Column<long>(nullable: true),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingReports_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_AbpUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingReports_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingReports_AbpUsers_FinderId",
                        column: x => x.FinderId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_AbpUsers_FindingCoordinatorId",
                        column: x => x.FindingCoordinatorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_AbpUsers_FindingManagerId",
                        column: x => x.FindingManagerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_AbpUsers_FindingOwnerId",
                        column: x => x.FindingOwnerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReports_FindingReportClassifications_FindingReportClassificationId",
                        column: x => x.FindingReportClassificationId,
                        principalTable: "FindingReportClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FindingReportRelatedBusinessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingReportId = table.Column<int>(nullable: true),
                    BusinessRiskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingReportRelatedBusinessRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedBusinessRisks_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FindingReportRelatedExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingReportId = table.Column<int>(nullable: true),
                    ExceptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingReportRelatedExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedExceptions_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedExceptions_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FindingReportRelatedIncidents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingReportId = table.Column<int>(nullable: true),
                    IncidentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingReportRelatedIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedIncidents_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FindingReportRelatedIncidents_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedBusinessRisks_BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedBusinessRisks_FindingReportId",
                table: "FindingReportRelatedBusinessRisks",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedExceptions_ExceptionId",
                table: "FindingReportRelatedExceptions",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedExceptions_FindingReportId",
                table: "FindingReportRelatedExceptions",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedIncidents_FindingReportId",
                table: "FindingReportRelatedIncidents",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedIncidents_IncidentId",
                table: "FindingReportRelatedIncidents",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_AssessmentId",
                table: "FindingReports",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_AssignedToUserId",
                table: "FindingReports",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_BusinessEntityId",
                table: "FindingReports",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_ControlRequirementId",
                table: "FindingReports",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FinderId",
                table: "FindingReports",
                column: "FinderId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingCoordinatorId",
                table: "FindingReports",
                column: "FindingCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingManagerId",
                table: "FindingReports",
                column: "FindingManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingOwnerId",
                table: "FindingReports",
                column: "FindingOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingReportClassificationId",
                table: "FindingReports",
                column: "FindingReportClassificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FindingReportRelatedBusinessRisks");

            migrationBuilder.DropTable(
                name: "FindingReportRelatedExceptions");

            migrationBuilder.DropTable(
                name: "FindingReportRelatedIncidents");

            migrationBuilder.DropTable(
                name: "FindingReports");
        }
    }
}
