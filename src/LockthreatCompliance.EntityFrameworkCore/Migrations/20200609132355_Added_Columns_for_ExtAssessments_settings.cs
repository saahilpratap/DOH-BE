using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_Columns_for_ExtAssessments_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessAuditFindings",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessAuditWorkPapers",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessBusinessRisks",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessExceptions",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessIncidents",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessInternalAssessments",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessInternalFindings",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessPreAuditQuestionnaire",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireBusinessEntityAcceptanceForAuditFinding",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireBusinessEntityAcceptanceForExtAssessment",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SkipBEAdminApprovalInExtAssessment",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SkipExtAssessReviewerApproval",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessAuditFindings",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessAuditWorkPapers",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessBusinessRisks",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessExceptions",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessIncidents",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessInternalAssessments",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessInternalFindings",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "AccessPreAuditQuestionnaire",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "RequireBusinessEntityAcceptanceForAuditFinding",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "RequireBusinessEntityAcceptanceForExtAssessment",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "SkipBEAdminApprovalInExtAssessment",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "SkipExtAssessReviewerApproval",
                table: "EntityApplicationSettings");
        }
    }
}
