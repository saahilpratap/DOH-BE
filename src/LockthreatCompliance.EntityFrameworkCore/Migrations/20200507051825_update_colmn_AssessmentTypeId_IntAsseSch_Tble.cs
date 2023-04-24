using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class update_colmn_AssessmentTypeId_IntAsseSch_Tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentType",
                table: "InternalAssessmentSchedules");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentTypeId",
                table: "InternalAssessmentSchedules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentSchedules_AssessmentTypeId",
                table: "InternalAssessmentSchedules",
                column: "AssessmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalAssessmentSchedules_AbpDynamicParameterValues_AssessmentTypeId",
                table: "InternalAssessmentSchedules",
                column: "AssessmentTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalAssessmentSchedules_AbpDynamicParameterValues_AssessmentTypeId",
                table: "InternalAssessmentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_InternalAssessmentSchedules_AssessmentTypeId",
                table: "InternalAssessmentSchedules");

            migrationBuilder.DropColumn(
                name: "AssessmentTypeId",
                table: "InternalAssessmentSchedules");

            migrationBuilder.AddColumn<string>(
                name: "AssessmentType",
                table: "InternalAssessmentSchedules",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
