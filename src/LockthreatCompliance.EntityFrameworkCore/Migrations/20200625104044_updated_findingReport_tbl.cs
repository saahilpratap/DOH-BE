using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_findingReport_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_Assessments_AssessmentId",
                table: "FindingReports");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_ExternalAssessments_AssessmentId",
                table: "FindingReports",
                column: "AssessmentId",
                principalTable: "ExternalAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_ExternalAssessments_AssessmentId",
                table: "FindingReports");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_Assessments_AssessmentId",
                table: "FindingReports",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
