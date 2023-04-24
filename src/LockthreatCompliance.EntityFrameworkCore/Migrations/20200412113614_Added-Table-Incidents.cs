using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableIncidents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    CloseDate = table.Column<DateTime>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentReviewers");

            migrationBuilder.DropTable(
                name: "Incidents");
        }
    }
}
