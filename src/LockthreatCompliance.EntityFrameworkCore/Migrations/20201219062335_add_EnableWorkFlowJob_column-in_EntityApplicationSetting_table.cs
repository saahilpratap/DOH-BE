using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_EnableWorkFlowJob_columnin_EntityApplicationSetting_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableWorkFlowJob",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableWorkFlowJob",
                table: "EntityApplicationSettings");
        }
    }
}
