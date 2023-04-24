using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableEntityGroupaddednewfiled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractPersonnel",
                table: "EntityGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ITSecurityStaff",
                table: "EntityGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberEmpWork",
                table: "EntityGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPersonnel",
                table: "EntityGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractPersonnel",
                table: "EntityGroups");

            migrationBuilder.DropColumn(
                name: "ITSecurityStaff",
                table: "EntityGroups");

            migrationBuilder.DropColumn(
                name: "NumberEmpWork",
                table: "EntityGroups");

            migrationBuilder.DropColumn(
                name: "TotalPersonnel",
                table: "EntityGroups");
        }
    }
}
