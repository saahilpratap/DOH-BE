using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableBusinessEntityaddedcolums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractPersonnel",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ITSecurityStaff",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberEmpWork",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPersonnel",
                table: "BusinessEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractPersonnel",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ITSecurityStaff",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "NumberEmpWork",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "TotalPersonnel",
                table: "BusinessEntities");
        }
    }
}
