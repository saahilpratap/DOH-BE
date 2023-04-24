using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Updated_Assess_Clarification_Tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClarificationCommentType",
                table: "AssessmentRequestClarifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClarificationType",
                table: "AssessmentRequestClarifications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClarificationCommentType",
                table: "AssessmentRequestClarifications");

            migrationBuilder.DropColumn(
                name: "ClarificationType",
                table: "AssessmentRequestClarifications");
        }
    }
}
