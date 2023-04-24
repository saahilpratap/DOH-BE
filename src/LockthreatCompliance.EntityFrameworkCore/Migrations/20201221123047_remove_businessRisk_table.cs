using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_businessRisk_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRiskRemediations_BusinessRisks_BusinessRiskId",
                table: "BusinessRiskRemediations");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisksCompensatingControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksCompensatingControls");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisksImpactedControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksImpactedControls");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisksMonitoringControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksMonitoringControls");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_BusinessRisks_BusinessRiskId",
                table: "DocumentPaths");

            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_FindingReportRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_IRMRelations_BusinessRisks_BusinessRiskId",
                table: "IRMRelations");

            migrationBuilder.DropTable(
                name: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_IRMRelations_BusinessRiskId",
                table: "IRMRelations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentRelatedBusinessRisks_BusinessRiskId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_FindingReportRelatedBusinessRisks_BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionRelatedBusinessRisks_BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_BusinessRiskId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisksMonitoringControls_BusinessRiskId",
                table: "BusinessRisksMonitoringControls");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisksImpactedControls_BusinessRiskId",
                table: "BusinessRisksImpactedControls");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisksCompensatingControls_BusinessRiskId",
                table: "BusinessRisksCompensatingControls");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRiskRemediations_BusinessRiskId",
                table: "BusinessRiskRemediations");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "IRMRelations");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "BusinessRisksMonitoringControls");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "BusinessRisksImpactedControls");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "BusinessRisksCompensatingControls");

            migrationBuilder.DropColumn(
                name: "BusinessRiskId",
                table: "BusinessRiskRemediations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "IRMRelations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "IncidentRelatedBusinessRisks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "DocumentPaths",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "BusinessRisksMonitoringControls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "BusinessRisksImpactedControls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "BusinessRisksCompensatingControls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRiskId",
                table: "BusinessRiskRemediations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BusinessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualClosureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessEntityId = table.Column<int>(type: "int", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriticalityId = table.Column<int>(type: "int", nullable: true),
                    ExpectedClosureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemediationCompleted = table.Column<bool>(type: "bit", nullable: false),
                    RemediationPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskAssessmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiskDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskImpactId = table.Column<int>(type: "int", nullable: true),
                    RiskLikelihoodId = table.Column<int>(type: "int", nullable: true),
                    RiskManagerId = table.Column<long>(type: "bigint", nullable: true),
                    RiskOwnerId = table.Column<long>(type: "bigint", nullable: true),
                    RiskTreatmentId = table.Column<int>(type: "int", nullable: true),
                    RiskTypeId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vulnerability = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpDynamicParameterValues_CriticalityId",
                        column: x => x.CriticalityId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskImpactId",
                        column: x => x.RiskImpactId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskLikelihoodId",
                        column: x => x.RiskLikelihoodId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpUsers_RiskManagerId",
                        column: x => x.RiskManagerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpUsers_RiskOwnerId",
                        column: x => x.RiskOwnerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTreatmentId",
                        column: x => x.RiskTreatmentId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTypeId",
                        column: x => x.RiskTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_BusinessRiskId",
                table: "IRMRelations",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedBusinessRisks_BusinessRiskId",
                table: "IncidentRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedBusinessRisks_BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionRelatedBusinessRisks_BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_BusinessRiskId",
                table: "DocumentPaths",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksMonitoringControls_BusinessRiskId",
                table: "BusinessRisksMonitoringControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksImpactedControls_BusinessRiskId",
                table: "BusinessRisksImpactedControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksCompensatingControls_BusinessRiskId",
                table: "BusinessRisksCompensatingControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRiskRemediations_BusinessRiskId",
                table: "BusinessRiskRemediations",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_BusinessEntityId",
                table: "BusinessRisks",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_CriticalityId",
                table: "BusinessRisks",
                column: "CriticalityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskImpactId",
                table: "BusinessRisks",
                column: "RiskImpactId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskLikelihoodId",
                table: "BusinessRisks",
                column: "RiskLikelihoodId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskManagerId",
                table: "BusinessRisks",
                column: "RiskManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskOwnerId",
                table: "BusinessRisks",
                column: "RiskOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskTreatmentId",
                table: "BusinessRisks",
                column: "RiskTreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_RiskTypeId",
                table: "BusinessRisks",
                column: "RiskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRiskRemediations_BusinessRisks_BusinessRiskId",
                table: "BusinessRiskRemediations",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisksCompensatingControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksCompensatingControls",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisksImpactedControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksImpactedControls",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisksMonitoringControls_BusinessRisks_BusinessRiskId",
                table: "BusinessRisksMonitoringControls",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_BusinessRisks_BusinessRiskId",
                table: "DocumentPaths",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReportRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "FindingReportRelatedBusinessRisks",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                table: "IncidentRelatedBusinessRisks",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IRMRelations_BusinessRisks_BusinessRiskId",
                table: "IRMRelations",
                column: "BusinessRiskId",
                principalTable: "BusinessRisks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
