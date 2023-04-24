using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedfieldinFidningTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditorRemark",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CAPAUpdateRequired",
                table: "FindingReports",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditorRemark",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "CAPAUpdateRequired",
                table: "FindingReports");
        }
    }
}
