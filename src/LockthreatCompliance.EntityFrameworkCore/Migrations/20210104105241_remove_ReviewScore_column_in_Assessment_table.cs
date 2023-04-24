using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_ReviewScore_column_in_Assessment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewScore",
                table: "Assessments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ReviewScore",
                table: "Assessments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
