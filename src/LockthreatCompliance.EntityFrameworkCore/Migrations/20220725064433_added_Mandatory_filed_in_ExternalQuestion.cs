using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_Mandatory_filed_in_ExternalQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mandatory",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Mandatory",
                table: "ExternalAssementQuestions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mandatory",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Mandatory",
                table: "ExternalAssementQuestions");
        }
    }
}
