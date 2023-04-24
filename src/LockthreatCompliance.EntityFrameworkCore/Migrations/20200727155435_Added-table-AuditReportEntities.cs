using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditReportEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditReportEntities",
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
                    AuditReportId = table.Column<int>(nullable: false),
                    Sampled = table.Column<bool>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ManDays = table.Column<int>(nullable: true),
                    SamplingSite = table.Column<string>(nullable: true),
                    Process = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditReportEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditReportEntities_AuditReports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "AuditReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditReportEntities_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditReportEntities_AuditReportId",
                table: "AuditReportEntities",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditReportEntities_BusinessEntityId",
                table: "AuditReportEntities",
                column: "BusinessEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditReportEntities");
        }
    }
}
