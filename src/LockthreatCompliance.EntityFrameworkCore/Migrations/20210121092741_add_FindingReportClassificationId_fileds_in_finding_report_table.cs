using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_FindingReportClassificationId_fileds_in_finding_report_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FindingReportClassificationId",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingReportClassificationId",
                table: "FindingReports",
                column: "FindingReportClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_FindingReportClassifications_FindingReportClassificationId",
                table: "FindingReports",
                column: "FindingReportClassificationId",
                principalTable: "FindingReportClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_FindingReportClassifications_FindingReportClassificationId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_FindingReportClassificationId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "FindingReportClassificationId",
                table: "FindingReports");
        }
    }
}
