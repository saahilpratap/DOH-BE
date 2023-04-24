using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatemigrationerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentTypeId",
                table: "ExternalAssessments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "ExternalAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendEmailNotification",
                table: "ExternalAssessments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SendSmsNotification",
                table: "ExternalAssessments",
                type: "bit",
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
    }
}
