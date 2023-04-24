using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_ExtAdtWrkPaper_tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditProjectId",
                table: "ExternalAssessmentAuditWorkPapers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentAuditWorkPapers_AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers",
                column: "AuditProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessmentAuditWorkPapers_AuditProjects_AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers",
                column: "AuditProjectId1",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessmentAuditWorkPapers_AuditProjects_AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessmentAuditWorkPapers_AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropColumn(
                name: "AuditProjectId1",
                table: "ExternalAssessmentAuditWorkPapers");
        }
    }
}
