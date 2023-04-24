using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedfiledinauditprojectRemoteDesktopAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RemoteDesktopAudit",
                table: "AuditProjects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoteDesktopAudit",
                table: "AuditProjects");
        }
    }
}
