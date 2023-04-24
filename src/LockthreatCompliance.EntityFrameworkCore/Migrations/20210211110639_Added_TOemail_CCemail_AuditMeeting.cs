using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_TOemail_CCemail_AuditMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCEmail",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToEmail",
                table: "AuditMeetings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCEmail",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "ToEmail",
                table: "AuditMeetings");
        }
    }
}
