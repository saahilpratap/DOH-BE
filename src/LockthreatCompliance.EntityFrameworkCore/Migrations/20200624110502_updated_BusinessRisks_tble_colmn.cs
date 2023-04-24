using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_BusinessRisks_tble_colmn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CriticalityId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RiskAssessmentDate",
                table: "BusinessRisks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RiskDetail",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskImpactId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskLikelihoodId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RiskManagerId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RiskOwnerId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskTreatmentId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskTypeId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessRisksCompensatingControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessRiskId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRisksCompensatingControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRisksCompensatingControls_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessRisksCompensatingControls_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRisksImpactedControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessRiskId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRisksImpactedControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRisksImpactedControls_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessRisksImpactedControls_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRisksMonitoringControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessRiskId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRisksMonitoringControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRisksMonitoringControls_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessRisksMonitoringControls_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksCompensatingControls_BusinessRiskId",
                table: "BusinessRisksCompensatingControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksCompensatingControls_ControlRequirementId",
                table: "BusinessRisksCompensatingControls",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksImpactedControls_BusinessRiskId",
                table: "BusinessRisksImpactedControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksImpactedControls_ControlRequirementId",
                table: "BusinessRisksImpactedControls",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksMonitoringControls_BusinessRiskId",
                table: "BusinessRisksMonitoringControls",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisksMonitoringControls_ControlRequirementId",
                table: "BusinessRisksMonitoringControls",
                column: "ControlRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_CriticalityId",
                table: "BusinessRisks",
                column: "CriticalityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskImpactId",
                table: "BusinessRisks",
                column: "RiskImpactId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskLikelihoodId",
                table: "BusinessRisks",
                column: "RiskLikelihoodId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpUsers_RiskManagerId",
                table: "BusinessRisks",
                column: "RiskManagerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpUsers_RiskOwnerId",
                table: "BusinessRisks",
                column: "RiskOwnerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTreatmentId",
                table: "BusinessRisks",
                column: "RiskTreatmentId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTypeId",
                table: "BusinessRisks",
                column: "RiskTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_CriticalityId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskImpactId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskLikelihoodId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpUsers_RiskManagerId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpUsers_RiskOwnerId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTreatmentId",
                table: "BusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_RiskTypeId",
                table: "BusinessRisks");

            migrationBuilder.DropTable(
                name: "BusinessRisksCompensatingControls");

            migrationBuilder.DropTable(
                name: "BusinessRisksImpactedControls");

            migrationBuilder.DropTable(
                name: "BusinessRisksMonitoringControls");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_CriticalityId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskImpactId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskLikelihoodId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskManagerId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskOwnerId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskTreatmentId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_RiskTypeId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "CriticalityId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskAssessmentDate",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskDetail",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskImpactId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskLikelihoodId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskManagerId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskOwnerId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskTreatmentId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "BusinessRisks");
        }
    }
}
