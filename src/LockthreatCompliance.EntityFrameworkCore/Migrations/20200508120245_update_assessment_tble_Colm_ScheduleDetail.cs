using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class update_assessment_tble_Colm_ScheduleDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternalAssessmentScheduleDetailId",
                table: "Assessments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleDetailId",
                table: "Assessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_InternalAssessmentScheduleDetailId",
                table: "Assessments",
                column: "InternalAssessmentScheduleDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_InternalAssessmentScheduleDetails_InternalAssessmentScheduleDetailId",
                table: "Assessments",
                column: "InternalAssessmentScheduleDetailId",
                principalTable: "InternalAssessmentScheduleDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_InternalAssessmentScheduleDetails_InternalAssessmentScheduleDetailId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_InternalAssessmentScheduleDetailId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "InternalAssessmentScheduleDetailId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "ScheduleDetailId",
                table: "Assessments");
        }
    }
}
