using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedfiledsForBusinessEntityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileAppName",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicIPAddress",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Suppliers",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VIPUsers",
                table: "BusinessEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileAppName",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "PublicIPAddress",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Suppliers",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "VIPUsers",
                table: "BusinessEntities");
        }
    }
}
