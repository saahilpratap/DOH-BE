using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updateExtassWorkPaperTble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralAttachmentCode",
                table: "ExternalAssessmentAuditWorkPapers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingAgendaCode",
                table: "ExternalAssessmentAuditWorkPapers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MgmtChecklistCode",
                table: "ExternalAssessmentAuditWorkPapers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralAttachmentCode",
                table: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropColumn(
                name: "MeetingAgendaCode",
                table: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropColumn(
                name: "MgmtChecklistCode",
                table: "ExternalAssessmentAuditWorkPapers");
        }
    }
}
