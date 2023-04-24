using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updateExtAss_Tbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ScheduleDetailId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
                column: "ExternalAssessmentScheduleDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
                column: "ExternalAssessmentScheduleDetailId",
                principalTable: "ExternalAssessmentScheduleDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "ScheduleDetailId",
                table: "ExternalAssessments");
        }
    }
}
