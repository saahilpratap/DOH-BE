using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_OriginalAuditProjectId_field_in_AuditProject_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginalAuditProjectId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_OriginalAuditProjectId",
                table: "AuditProjects",
                column: "OriginalAuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AuditProjects_OriginalAuditProjectId",
                table: "AuditProjects",
                column: "OriginalAuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AuditProjects_OriginalAuditProjectId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_OriginalAuditProjectId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "OriginalAuditProjectId",
                table: "AuditProjects");
        }
    }
}
