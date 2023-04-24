using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_performance_column_in_AuditReports_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Performance1",
                table: "AuditReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Performance2",
                table: "AuditReports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Performance1",
                table: "AuditReports");

            migrationBuilder.DropColumn(
                name: "Performance2",
                table: "AuditReports");
        }
    }
}
