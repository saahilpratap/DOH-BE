using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_SystemDynamicParameterList_column_in_EntityApplicationSetting_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemDynamicParameterList",
                table: "EntityApplicationSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemDynamicParameterList",
                table: "EntityApplicationSettings");
        }
    }
}
