using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AllResponseCompleted_column_in_Assessment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllResponseCompleted",
                table: "Assessments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllResponseCompleted",
                table: "Assessments");
        }
    }
}
