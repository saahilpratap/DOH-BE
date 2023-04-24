using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_table_Incidents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "IRMRelations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "IncidentRemediations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "IncidentRelatedExceptions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "IncidentRelatedBusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "FindingReportRelatedIncidents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IncidentTypeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DetectionDateTime = table.Column<DateTime>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RootCause = table.Column<string>(nullable: true),
                    Remediation = table.Column<string>(nullable: true),
                    Severity = table.Column<int>(nullable: false),
                    IncidentImpactId = table.Column<int>(nullable: false),
                    Correction = table.Column<string>(nullable: true),
                    Prevention = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CloseDate = table.Column<DateTime>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    ClosedByUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_AbpUsers_ClosedByUserId",
                        column: x => x.ClosedByUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_IncidentImpacts_IncidentImpactId",
                        column: x => x.IncidentImpactId,
                        principalTable: "IncidentImpacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_IncidentTypes_IncidentTypeId",
                        column: x => x.IncidentTypeId,
                        principalTable: "IncidentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentReviewers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(nullable: false),
                    ReviewerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentReviewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentReviewers_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentReviewers_AbpUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_IncidentId",
                table: "IRMRelations",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRemediations_IncidentId",
                table: "IncidentRemediations",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedExceptions_IncidentId",
                table: "IncidentRelatedExceptions",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedBusinessRisks_IncidentId",
                table: "IncidentRelatedBusinessRisks",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReportRelatedIncidents_IncidentId",
                table: "FindingReportRelatedIncidents",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_IncidentId",
                table: "DocumentPaths",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReviewers_IncidentId",
                table: "IncidentReviewers",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReviewers_ReviewerId",
                table: "IncidentReviewers",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_BusinessEntityId",
                table: "Incidents",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ClosedByUserId",
                table: "Incidents",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_IncidentImpactId",
                table: "Incidents",
                column: "IncidentImpactId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_IncidentTypeId",
                table: "Incidents",
                column: "IncidentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_Incidents_IncidentId",
                table: "DocumentPaths",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReportRelatedIncidents_Incidents_IncidentId",
                table: "FindingReportRelatedIncidents",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentRelatedBusinessRisks_Incidents_IncidentId",
                table: "IncidentRelatedBusinessRisks",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentRelatedExceptions_Incidents_IncidentId",
                table: "IncidentRelatedExceptions",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentRemediations_Incidents_IncidentId",
                table: "IncidentRemediations",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IRMRelations_Incidents_IncidentId",
                table: "IRMRelations",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_Incidents_IncidentId",
                table: "DocumentPaths");

            migrationBuilder.DropForeignKey(
                name: "FK_FindingReportRelatedIncidents_Incidents_IncidentId",
                table: "FindingReportRelatedIncidents");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentRelatedBusinessRisks_Incidents_IncidentId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentRelatedExceptions_Incidents_IncidentId",
                table: "IncidentRelatedExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentRemediations_Incidents_IncidentId",
                table: "IncidentRemediations");

            migrationBuilder.DropForeignKey(
                name: "FK_IRMRelations_Incidents_IncidentId",
                table: "IRMRelations");

            migrationBuilder.DropTable(
                name: "IncidentReviewers");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_IRMRelations_IncidentId",
                table: "IRMRelations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentRemediations_IncidentId",
                table: "IncidentRemediations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentRelatedExceptions_IncidentId",
                table: "IncidentRelatedExceptions");

            migrationBuilder.DropIndex(
                name: "IX_IncidentRelatedBusinessRisks_IncidentId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_FindingReportRelatedIncidents_IncidentId",
                table: "FindingReportRelatedIncidents");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_IncidentId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "IRMRelations");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "IncidentRemediations");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "IncidentRelatedExceptions");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "FindingReportRelatedIncidents");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "DocumentPaths");
        }
    }
}
