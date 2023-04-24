using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addmigrationtableemailNotificationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bcc",
                table: "EmailNotificationTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cc",
                table: "EmailNotificationTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "EmailNotificationTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bcc",
                table: "EmailNotificationTemplates");

            migrationBuilder.DropColumn(
                name: "Cc",
                table: "EmailNotificationTemplates");

            migrationBuilder.DropColumn(
                name: "To",
                table: "EmailNotificationTemplates");
        }
    }
}
