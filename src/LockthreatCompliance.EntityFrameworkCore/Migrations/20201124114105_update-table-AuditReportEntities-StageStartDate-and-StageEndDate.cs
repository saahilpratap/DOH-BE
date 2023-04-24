using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuditReportEntitiesStageStartDateandStageEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StageEndDate",
                table: "AuditReportEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StageManDays",
                table: "AuditReportEntities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StageStartDate",
                table: "AuditReportEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageEndDate",
                table: "AuditReportEntities");

            migrationBuilder.DropColumn(
                name: "StageManDays",
                table: "AuditReportEntities");

            migrationBuilder.DropColumn(
                name: "StageStartDate",
                table: "AuditReportEntities");
        }
    }
}
