using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class update_assessment_tble_Colm_assessmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentTypeId",
                table: "Assessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_AssessmentTypeId",
                table: "Assessments",
                column: "AssessmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_AbpDynamicParameterValues_AssessmentTypeId",
                table: "Assessments",
                column: "AssessmentTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_AbpDynamicParameterValues_AssessmentTypeId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_AssessmentTypeId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "AssessmentTypeId",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
