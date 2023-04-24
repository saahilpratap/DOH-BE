using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableExternalAssementScheduleEntityGroupaddedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExtGenerated",
                table: "ExternalAssessmentScheduleEntityGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtGenerated",
                table: "ExternalAssessmentScheduleEntityGroups");
        }
    }
}
