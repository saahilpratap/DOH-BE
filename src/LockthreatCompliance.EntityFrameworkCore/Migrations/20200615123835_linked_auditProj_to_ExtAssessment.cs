using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class linked_auditProj_to_ExtAssessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_AuditProjectId",
                table: "ExternalAssessments",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_AuditProjects_AuditProjectId",
                table: "ExternalAssessments",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_AuditProjects_AuditProjectId",
                table: "ExternalAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_AuditProjectId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "ExternalAssessments");
        }
    }
}
