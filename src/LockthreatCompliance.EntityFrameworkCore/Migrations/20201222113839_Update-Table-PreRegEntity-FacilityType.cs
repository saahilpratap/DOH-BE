using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdateTablePreRegEntityFacilityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacilitySubTypeName",
                table: "PreRegEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacilityTypeName",
                table: "PreRegEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilitySubTypeName",
                table: "PreRegEntities");

            migrationBuilder.DropColumn(
                name: "FacilityTypeName",
                table: "PreRegEntities");
        }
    }
}
