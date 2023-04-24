using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_Extassesssment_tbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralComplianceAssessmentId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_GeneralComplianceAssessmentId",
                table: "ExternalAssessments",
                column: "GeneralComplianceAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_GeneralComplianceAssessments_GeneralComplianceAssessmentId",
                table: "ExternalAssessments",
                column: "GeneralComplianceAssessmentId",
                principalTable: "GeneralComplianceAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_GeneralComplianceAssessments_GeneralComplianceAssessmentId",
                table: "ExternalAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_GeneralComplianceAssessmentId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "GeneralComplianceAssessmentId",
                table: "ExternalAssessments");
        }
    }
}
