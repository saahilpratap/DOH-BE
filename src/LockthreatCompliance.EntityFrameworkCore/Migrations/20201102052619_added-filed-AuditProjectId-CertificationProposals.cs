using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedfiledAuditProjectIdCertificationProposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "CertificationProposals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificationProposals_AuditProjectId",
                table: "CertificationProposals",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificationProposals_AuditProjects_AuditProjectId",
                table: "CertificationProposals",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificationProposals_AuditProjects_AuditProjectId",
                table: "CertificationProposals");

            migrationBuilder.DropIndex(
                name: "IX_CertificationProposals_AuditProjectId",
                table: "CertificationProposals");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "CertificationProposals");
        }
    }
}
