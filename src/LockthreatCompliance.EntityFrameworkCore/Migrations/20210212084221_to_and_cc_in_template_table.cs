using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class to_and_cc_in_template_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateCc",
                table: "Templates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateTo",
                table: "Templates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateCc",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "TemplateTo",
                table: "Templates");
        }
    }
}
