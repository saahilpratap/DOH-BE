using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_mandetory_fileds_of_finding_report_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FindingReportClassificationId",
                table: "FindingReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
