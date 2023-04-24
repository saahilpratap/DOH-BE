using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_relation_between_auditReport_in_AuditReportEntities_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReportEntities_AuditReports_AuditReportId",
                table: "AuditReportEntities");

            migrationBuilder.DropIndex(
                name: "IX_AuditReportEntities_AuditReportId",
                table: "AuditReportEntities");

            migrationBuilder.DropColumn(
                name: "AuditReportId",
                table: "AuditReportEntities");

            migrationBuilder.CreateIndex(
                name: "IX_AuditReportEntities_AuditProjectId",
                table: "AuditReportEntities",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReportEntities_AuditProjects_AuditProjectId",
                table: "AuditReportEntities",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReportEntities_AuditProjects_AuditProjectId",
                table: "AuditReportEntities");

            migrationBuilder.DropIndex(
                name: "IX_AuditReportEntities_AuditProjectId",
                table: "AuditReportEntities");

            migrationBuilder.AddColumn<int>(
                name: "AuditReportId",
                table: "AuditReportEntities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AuditReportEntities_AuditReportId",
                table: "AuditReportEntities",
                column: "AuditReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReportEntities_AuditReports_AuditReportId",
                table: "AuditReportEntities",
                column: "AuditReportId",
                principalTable: "AuditReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
