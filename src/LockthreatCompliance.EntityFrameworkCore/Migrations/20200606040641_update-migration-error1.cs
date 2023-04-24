using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatemigrationerror1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentTypeId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendEmailNotification",
                table: "ExternalAssessments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SendSmsNotification",
                table: "ExternalAssessments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_AssessmentTypeId",
                table: "ExternalAssessments",
                column: "AssessmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_AbpDynamicParameterValues_AssessmentTypeId",
                table: "ExternalAssessments",
                column: "AssessmentTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_AbpDynamicParameterValues_AssessmentTypeId",
                table: "ExternalAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_AssessmentTypeId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "AssessmentTypeId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "SendEmailNotification",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "SendSmsNotification",
                table: "ExternalAssessments");
        }
    }
}
