using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditProjectId_in_AuditReportEntities_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "AuditReportEntities",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AuditReportEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "AuditReportEntities");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AuditReportEntities");
        }
    }
}
