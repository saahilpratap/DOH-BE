using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_AuditTeam_EXtAss_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditeeTeam",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditorTeam",
                table: "ExternalAssessments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditeeTeam",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "AuditorTeam",
                table: "ExternalAssessments");
        }
    }
}
