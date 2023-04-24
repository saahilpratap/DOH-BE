using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableInternalAssessmentScheduleBusinessEntityaddedcolumsEntityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "InternalAssessmentScheduleBusinessEntitys",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "InternalAssessmentScheduleBusinessEntitys");
        }
    }
}
