using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class update_IntAssSchDetail_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentType",
                table: "InternalAssessmentScheduleDetails");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentScheduleDetails_AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails",
                column: "AssessmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalAssessmentScheduleDetails_AbpDynamicParameterValues_AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails",
                column: "AssessmentTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalAssessmentScheduleDetails_AbpDynamicParameterValues_AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails");

            migrationBuilder.DropIndex(
                name: "IX_InternalAssessmentScheduleDetails_AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails");

            migrationBuilder.DropColumn(
                name: "AssessmentTypeId",
                table: "InternalAssessmentScheduleDetails");

            migrationBuilder.AddColumn<string>(
                name: "AssessmentType",
                table: "InternalAssessmentScheduleDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
